using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServer;
using ParkInspect.ViewModel;

namespace ParkInspect
{
    public class DataSynchroniser
    {
        private static string[] syncedTables =
        {
            "FormField"            
        };

        private SyncOrchestrator orchestrator;

        private SqlConnection local;
        private SqlConnection central;

        public DataSynchroniser()
        {
            orchestrator = new SyncOrchestrator();

            local =
                new SqlConnection(
                    @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\LocalDb.mdf';Integrated Security=True");

            central =
                new SqlConnection(
                    @"Data Source=bhooff.database.windows.net;Initial Catalog=Parkinspect;Persist Security Info=True;User ID=bhooff;Password=Wachtwoord123");


            //orchestrator.LocalProvider = new SqlSyncProvider("local", local);
            //orchestrator.RemoteProvider = new SqlSyncProvider("central", central);
            //orchestrator.Direction = SyncDirectionOrder.UploadAndDownload;

            //provisionDatabase(local, "local");
            //provisionDatabase(central, "central");


            //((SqlSyncProvider) orchestrator.LocalProvider).ApplyChangeFailed +=
              //  new EventHandler<DbApplyChangeFailedEventArgs>(onSyncFail);
        }

        public void setupUserFilters(Employee employee)
        {
            provisionLocalDatabase(local, employee);
        }

        private void onSyncFail(object sender, DbApplyChangeFailedEventArgs e)
        {
            Debug.Write(e.Error.Message);
        }

        public void sync()
        {
            /* synchronise in a seperate thread, to not freeze the entire application
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                orchestrator.Synchronize();
            }).Start();
            */
            if (ViewModelLocator.CheckForInternetConnection())
            {
                try
                {
                    orchestrator.Synchronize();
                    MessageBox.Show("Synchronisatie voltooid!", "De synchronisatie is succesvol voltooid.");                   
                }
                catch (Exception)
                {
                    MessageBox.Show("Fatale fout",
                        "Er is een fout opgetreden in de sync configuratie! Neem contact op met uw systeembeheerder.");
                }
            }
            else
            {
                MessageBox.Show("Geen verbinding!",
                    "U dient met internet verbonden te zijn om te synchroniseren.");
            }
        }

        public void synchronise()
        {
            //Best veel nesting
            if (ViewModelLocator.CheckForInternetConnection())
            {
                try
                {

                    // _syncService.InitializeDatabase();

                    ProvisionServer();
                    ProvisionClient();

                    // create a connection to the SyncCompactDB database
                    var clientConn = new SqlConnection(local.ConnectionString);

                    // create a connection to the SyncDB server database
                    var serverConn = new SqlConnection(central.ConnectionString);

                    // create the sync orhcestrator
                    var syncOrchestrator = new SyncOrchestrator
                    {
                        // set local provider of orchestrator to a CE sync provider associated with the 
                        // ProductsScope in the SyncCompactDB compact client database
                        LocalProvider = new SqlSyncProvider("ParkInspect", clientConn),
                        // set the remote provider of orchestrator to a server sync provider associated with
                        // the ProductsScope in the SyncDB server database
                        RemoteProvider = new SqlSyncProvider("ParkInspect", serverConn),
                        // set the direction of sync session to Upload and Download
                        Direction = SyncDirectionOrder.UploadAndDownload
                    };

                    // subscribe for errors that occur when applying changes to the client
                    ((SqlSyncProvider)syncOrchestrator.LocalProvider).ApplyChangeFailed += onSyncFail;

                    // execute the synchronization process
                    syncOrchestrator.Synchronize();

                    MessageBox.Show("Synchronisatie voltooid!", "De synchronisatie is succesvol voltooid.");
                }
                catch (Exception)
                {
                    MessageBox.Show("Fatale fout", "Er is een fout opgetreden in de sync configuratie! Neem contact op met uw systeembeheerder.");
                }

            }
            else
            {
                MessageBox.Show("Geen verbinding!", "U dient met internet verbonden te zijn om te synchroniseren.");
            }
        }

        private void provisionLocalDatabase(SqlConnection connection, Employee employee)
        {
            try
            {
                DbSyncScopeDescription scopeDescription = new DbSyncScopeDescription("local");
                foreach (String s in syncedTables)
                {
                    DbSyncTableDescription table = SqlSyncDescriptionBuilder.GetDescriptionForTable(s, connection);
                    scopeDescription.Tables.Add(table);
                }
                SqlSyncScopeProvisioning provisioning = new SqlSyncScopeProvisioning(connection, scopeDescription);
                provisioning.SetCreateTableDefault(DbSyncCreationOption.CreateOrUseExisting);

                provisioning.Tables["Inspection"].AddFilterColumn("id");
                provisioning.Tables["Inspection"].FilterClause = "[id] = '" + employee.id + "'";

                provisioning.Apply();
            }
            catch (Exception)
            {

            }
        }

        private void deprovisionLocalDatabase(SqlConnection connection)
        {
            // remove provisioning from a database
            try
            {
                orchestrator.LocalProvider = null;
                orchestrator.LocalProvider = new SqlSyncProvider("local", local);
                SqlSyncScopeDeprovisioning deprovisioner = new SqlSyncScopeDeprovisioning(connection);
                deprovisioner.DeprovisionScope("local");
            }
            catch (Exception)
            {

            }
        }

        private void provisionDatabase(SqlConnection connection, string scopeName)
        {
            // not sure how to properly check if a database is provisioned, so we try-catch it unless a better solution is found
            try
            {
                DbSyncScopeDescription scopeDescription = new DbSyncScopeDescription(scopeName);
                foreach (String s in syncedTables)
                {
                    DbSyncTableDescription table = SqlSyncDescriptionBuilder.GetDescriptionForTable(s, connection);
                    scopeDescription.Tables.Add(table);
                }

                SqlSyncScopeProvisioning provisioning = new SqlSyncScopeProvisioning(connection, scopeDescription);
                provisioning.SetCreateTableDefault(DbSyncCreationOption.CreateOrUseExisting);
                provisioning.Apply();
            }
            catch (Exception)
            {
                return;
            }           
        }

        private void ProvisionServer()
        {
            var serverConn = new SqlConnection(central.ConnectionString);

            // define a new scope named ParkInspectScope
            var scopeDesc = new DbSyncScopeDescription("ParkInspect");
            
            foreach (var name in syncedTables)
            {
                // get the description of the Products table from SyncDB dtabase
                var tableDesc = SqlSyncDescriptionBuilder.GetDescriptionForTable(name, serverConn);

                // add the table description to the sync scope definition
                scopeDesc.Tables.Add(tableDesc);
            }

            // create a server scope provisioning object based on the ProductScope
            var serverProvision = new SqlSyncScopeProvisioning(serverConn, scopeDesc);

            // skipping the creation of table since table already exists on server
            serverProvision.SetCreateTableDefault(DbSyncCreationOption.Skip);

            // start the provisioning process
            if (!serverProvision.ScopeExists("ParkInspect"))
                serverProvision.Apply();
        }

        private void ProvisionClient()
        {
            // create a connection to the SyncCompactDB database
            var clientConn = new SqlConnection(local.ConnectionString);

            // create a connection to the SyncDB server database
            var serverConn = new SqlConnection(central.ConnectionString);

            // get the description of ProductsScope from the SyncDB server database
            var scopeDesc = SqlSyncDescriptionBuilder.GetDescriptionForScope("ParkInspect", serverConn);

            // create CE provisioning object based on the ProductsScope
            var clientProvision = new SqlSyncScopeProvisioning(clientConn, scopeDesc);
            // starts the provisioning process
            if (!clientProvision.ScopeExists("ParkInspect"))
                clientProvision.Apply();
        }
    }
}
