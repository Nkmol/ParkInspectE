using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Data.SqlClient;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.Server;
using Microsoft.Synchronization.Data.SqlServer;
using Microsoft.Synchronization.Data.SqlServerCe;

namespace ParkInspect.DataSync
{
    public class DataSynchroniser
    {

        private static string[] syncedTables =
        {


            "[Region]",
            "[Client]",
            "[Role]",
            "[State]",
            "[Employee_status]",
            "[Template]",
            "[Datatype]",
            "[ReportFieldType]",
            "[Contactperson]",
            "[Parkinglot]",
            "[Employee]",
            "[Inspector]",
            "[Asignment]",
            "[Form]",
            "[Inspection]",
            "[Inspector_has_Inspection]",
            "[Absence]",
            "[Image]",
            "[Report]",
            "[Field]",
            "[Formfield]",

        };

        private SyncOrchestrator orchestrator;

        private SqlConnection local;
        private SqlConnection central;

        public DataSynchroniser()
        {
            orchestrator = new SyncOrchestrator();

            local = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='P:\Avans\Blok 6\PROJECT\ParkInspectE(Templates)\ParkInspect\ParkInspect\LocalDatabase.mdf';Integrated Security=True");

            central = new SqlConnection(@"Data Source=bhooff.database.windows.net;Initial Catalog=Parkinspect;Persist Security Info=True;User ID=bhooff;Password=Wachtwoord123");


            orchestrator.LocalProvider = new SqlSyncProvider("local", local);
            orchestrator.RemoteProvider = new SqlSyncProvider("central", central);
            orchestrator.Direction = SyncDirectionOrder.UploadAndDownload;

            provisionDatabase(local, "local");
            provisionDatabase(central, "central");


            ((SqlSyncProvider)orchestrator.LocalProvider).ApplyChangeFailed += new EventHandler<DbApplyChangeFailedEventArgs>(onSyncFail);
        }

        public void setupUserFilters(Employee employee)
        {
            provisionLocalDatabase(local, employee);
        }

        private void onSyncFail(object sender, DbApplyChangeFailedEventArgs e)
        {
            Debug.Write(e.Error.Message);
        }

        public void synchronise()
        {
            // synchronise in a seperate thread, to not freeze the entire application
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                orchestrator.Synchronize();
            }).Start();
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

    }
}
