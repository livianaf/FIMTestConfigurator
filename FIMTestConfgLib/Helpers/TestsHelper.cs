using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Security.Principal;

//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public class TestsHelper {
        //_______________________________________________________________________________________________________________________
        #region SQL Creación Tablas
        private static string SQLTablesCreation = @"
CREATE TABLE if not exists [testoutputset] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`testid`	INTEGER NOT NULL,
	`outputsetid`	INTEGER NOT NULL,
	`order`	INTEGER
);
CREATE TABLE if not exists [test] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`name`	TEXT NOT NULL,
	`desc`	TEXT DEFAULT 'Description & requirements',
	`configid`	INTEGER,
	`inputsetid`	INTEGER,
	`sourcemaid`	INTEGER,
	`scriptid`	INTEGER,
	`commit`	INTEGER NOT NULL DEFAULT 0,
	`delta`	INTEGER NOT NULL DEFAULT 1
);
CREATE TABLE if not exists `script` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`name`	TEXT,
	`desc`	TEXT,
	`filecontent`	TEXT
);
CREATE TABLE if not exists [source] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`name`	TEXT NOT NULL,
	`server`	TEXT,
	`user`	TEXT,
	`password`	TEXT,
	`authtype`	TEXT
);
CREATE TABLE if not exists [outputset] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`name`	TEXT NOT NULL,
	`detail`	TEXT,
	`destmaid`	INTEGER,
	`type`	TEXT DEFAULT 'exportflow'
);
CREATE TABLE if not exists [managementagent] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`name`	TEXT NOT NULL,
	`sourceid`	INTEGER
);
CREATE TABLE if not exists [inputset] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`name`	TEXT NOT NULL,
	`detail`	TEXT,
	`dn`	TEXT
);
CREATE TABLE if not exists [grouptest] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`groupid`	INTEGER NOT NULL,
	`testid`	INTEGER NOT NULL,
	`order`	INTEGER
);
CREATE TABLE if not exists [groups] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`name`	TEXT NOT NULL
);
CREATE TABLE if not exists [config] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`name`	TEXT NOT NULL,
	`desc`	TEXT,
	`filecontent`	TEXT
);
CREATE TABLE if not exists [batch] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`name`	TEXT NOT NULL
);
CREATE TABLE if not exists [batchgroup] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`batchid`	INTEGER NOT NULL,
	`groupid`	INTEGER NOT NULL,
	`order`	INTEGER
);
CREATE TABLE if not exists [location] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`name`	TEXT NOT NULL,
	`desc`	TEXT
);
CREATE TABLE if not exists [variable] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`name`	TEXT NOT NULL,
	`desc`	TEXT
);
CREATE TABLE if not exists [locationvariable] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`locationid`	INTEGER NOT NULL,
	`variableid`	INTEGER NOT NULL,
	`value`	TEXT
);
CREATE TABLE if not exists [hiddenitem] (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`name`	TEXT NOT NULL
);";

        #endregion
        #region Default Detail Values
        private static string defaultScript = @"
#_______________________________________________________________________________________________________
param ( [ValidateSet('before','after')] [string]$phase, [ValidateSet('ok','error')] [string]$lastResult, 
        [string]$dn, [string]$user, [string]$password, [string]$server, [string]$authType )
#_______________________________________________________________________________________________________
function Start-Script(){
    # Use Write-Output, Write-Warning and / or Write-Error if they are necessary
    # If Write-Error is used and $phase is 'before' then the Test execution is aborted
    if($phase -eq 'before'){
        # Write-Output 'Tasks to be executed before test'
        }
    if($phase -eq 'after'){
        # Write-Output 'Tasks to be executed after test'
        }
    }
#_______________________________________________________________________________________________________
Start-Script
";
        private static string defaultInputDetail = @"
// comments starts with '/'
// Sintax: list of attribute|value pairs one per line. 
// Empty lines are ignored.
";
        private static string defaultOutputDetail = @"
// comments starts with '/'
// Sintax: list of attribute|value pairs one per line. 
// Empty lines are ignored.
// Allowed values:
// - <empty>                     -> check if value is empty or not present.
// - <literal value>             -> check if value has this value.
// - *                           -> check if value is present.
// - regexp:<regular expression> -> check if value match regular expression.
";
        private static string defaultLocation = System.Environment.MachineName;

        #endregion
        //_______________________________________________________________________________________________________________________
        public static bool IsAdministrator => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        //_______________________________________________________________________________________________________________________
        private static SQLiteHelper sqliteHelper = null;
        public static string LastErrorMessages => sqliteHelper?.LastErrorMessages;
        //_______________________________________________________________________________________________________________________
        public static bool Initialized { get { return sqliteHelper != null; } }
        //_______________________________________________________________________________________________________________________
        public static void Init( string BDName ) {
            sqliteHelper = new SQLiteHelper(BDName);
            foreach(string sCreate in SQLTablesCreation.Split(';'))
                if(!string.IsNullOrWhiteSpace(sCreate)) sqliteHelper.ExecuteNonQuery(sCreate);
            // añade columna Desc a Config si es necesario
            if (1 != (long)sqliteHelper.ExecuteScalar("select count(*) from sqlite_master where name=@param1 and sql like '%desc%';",GetParameters("config"))) 
                sqliteHelper.ExecuteNonQuery("ALTER TABLE[config] ADD COLUMN `desc` TEXT;");
            if (1 != (long)sqliteHelper.ExecuteScalar("select count(*) from sqlite_master where name=@param1 and sql like '%desc%';", GetParameters("test")))
                sqliteHelper.ExecuteNonQuery("ALTER TABLE[test] ADD COLUMN `desc` TEXT DEFAULT 'Description & requirements';");
            if (1 != (long)sqliteHelper.ExecuteScalar("select count(*) from sqlite_master where name=@param1 and sql like '%scriptid%';", GetParameters("test")))
                sqliteHelper.ExecuteNonQuery("ALTER TABLE[test] ADD COLUMN `scriptid` TEXT;");

            // Add default outputsets
            foreach (string name in TestsHelper.getOutputSetTypes()) {
                if (!Enum.TryParse(name, out OutputSet.AllowedTypes type)) continue;
                if (((OutputSet.TypeRequires)type | OutputSet.TypeRequires.Nothing) != OutputSet.TypeRequires.Nothing) continue;
                if (0 != (long)sqliteHelper.ExecuteScalar("select count(*) from outputset where type=@param1;", GetParameters(name))) continue;
                updateID("outputset", createID("outputset", name), "type", name);
                }
            }
        //_______________________________________________________________________________________________________________________
        public static void Close() { sqliteHelper.CloseDB(); sqliteHelper = null; }
        #region getXXXByID para TestObjectBase LinkedObject
        //_______________________________________________________________________________________________________________________
        public static Batch getBatchByID( long Id ) {
            using (DataTable dt = GetItems("batch", Id)) {
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0) {
                    DataRow dr = dt.Rows[0];
                    return new Batch((long)dr["id"], dr["name"].ToString(), getAllGroupsInBatch((long)dr["id"]));
                    }
                }
            return null;
            }
        //_______________________________________________________________________________________________________________________
        public static Group getGroupByID(long Id) {
            using (DataTable dt = GetItems("groups", Id)) {
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0) {
                    DataRow dr = dt.Rows[0];
                    return new Group((long)dr["id"], dr["name"].ToString(), getAllTestsInGroup((long)dr["id"]));
                    }
                }
            return null;
            }
        //_______________________________________________________________________________________________________________________
        public static Test getTestByID( long Id ) {
            using (DataTable dt = GetItems("test", Id)) {
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0) {
                    DataRow dr = dt.Rows[0];

                    Script script = dr.IsNull("scriptid") ? null : getScriptByID((long)dr["scriptid"]);
                    Config cfg = dr.IsNull("configid") ? null : getConfigByID((long)dr["configid"]);
                    InputSet iset = dr.IsNull("inputsetid") ? null : getInputSetByID((long)dr["inputsetid"]);
                    ManagementAgentInfo ma = dr.IsNull("sourcemaid") ? null : getMAByID((long)dr["sourcemaid"]);
                    List<OutputSet> osets = getAllOutputSetsInTest((long)dr["id"]);

                    return new Test((long)dr["id"], (string)dr["name"], (string)(dr.IsNull("desc")?"":dr["desc"]), cfg, script, iset, osets, ma, (long)dr["delta"] == 1, (long)dr["commit"] == 1);
                    }
                }
            return null;
            }
        //_______________________________________________________________________________________________________________________
        public static Config getConfigByID( long Id ) {
            using (DataTable dt = GetItems("config", Id)) {
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0) {
                    DataRow dr = dt.Rows[0];
                    return new Config((long)dr["id"], dr["name"].ToString(), dr["desc"].ToString(), dr["filecontent"].ToString());
                    }
                }
            return null;
            }
        //_______________________________________________________________________________________________________________________
        public static Script getScriptByID( long Id ) {
            using (DataTable dt = GetItems("script", Id)) {
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0) {
                    DataRow dr = dt.Rows[0];
                    return new Script((long)dr["id"], dr["name"].ToString(), dr["desc"].ToString(), dr["filecontent"].ToString());
                    }
                }
            return null;
            }
        //_______________________________________________________________________________________________________________________
        public static Source getSourceByID( long Id ) {
            using (DataTable dt = GetItems("source", Id)) {
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0) {
                    DataRow dr = dt.Rows[0];
                    return new Source((long)dr["id"], dr["name"].ToString(), dr["server"].ToString(), dr["user"].ToString(), dr["password"].ToString(), dr["authtype"].ToString());
                    }
                }
            return null;
            }
        //_______________________________________________________________________________________________________________________
        public static ManagementAgentInfo getMAByID( long Id ) {
            using (DataTable dt = GetItems("managementagent", Id)) {
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0) {
                    DataRow dr = dt.Rows[0];
                    return new ManagementAgentInfo((long)dr["id"], dr["name"].ToString(), dr["sourceid"].ToString() == "" ? null : getSourceByID((long)dr["sourceid"]));
                    }
                }
            return null;
            }
        //_______________________________________________________________________________________________________________________
        public static InputSet getInputSetByID( long Id) {
            using (DataTable dt = GetItems("inputset", Id)) {
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0) {
                    DataRow dr = dt.Rows[0];
                    return new InputSet((long)dr["id"], dr["name"].ToString(), dr["detail"].ToString(), dr["dn"].ToString());
                    }
                }
            return null;
            }
        //_______________________________________________________________________________________________________________________
        public static OutputSet getOutputSetByID( long Id) {
            using (DataTable dt = GetItems("outputset", Id)) {
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0) {
                    DataRow dr = dt.Rows[0];
                    return new OutputSet((long)dr["id"], dr["name"].ToString(), dr["detail"].ToString(), dr["type"].ToString(), dr.IsNull("destmaid") ? null : getMAByID((long)dr["destmaid"]));
                    }
                }
            return null;
            }
        //_______________________________________________________________________________________________________________________
        public static Location getLocationByID(long Id) {
            using (DataTable dt = GetItems("location", Id)) {
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0) {
                    DataRow dr = dt.Rows[0];
                    return new Location((long)dr["id"], dr["name"].ToString(), dr["desc"].ToString(), getAllVariablesInLocation((long)dr["id"]));
                    }
                }
            return null;
            }
        //_______________________________________________________________________________________________________________________
        public static Variable getVariableByID(long Id) {
            using (DataTable dt = GetItems("variable", Id)) {
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0) {
                    DataRow dr = dt.Rows[0];
                    return new Variable((long)dr["id"], dr["name"].ToString(), null, dr["desc"].ToString());
                    }
                }
            return null;
            }
        #endregion
        #region getAllXXX
        //_______________________________________________________________________________________________________________________
        public static List<Batch> getAllBatches() {
            using (DataTable dt = GetItems("batch")) {
                List<Batch> list = new List<Batch>();
                foreach (DataRow dr in dt.Rows) list.Add(new Batch((long)dr["id"], dr["name"].ToString(), getAllGroupsInBatch((long)dr["id"])));
                return list;
                }
            }
        //_______________________________________________________________________________________________________________________
        public static List<Group> getAllGroups() {
            using(DataTable dt = GetItems("groups")){
                List<Group> list = new List<Group>();
                foreach (DataRow dr in dt.Rows) list.Add(new Group((long)dr["id"], dr["name"].ToString(), getAllTestsInGroup((long)dr["id"])));
                return list;
                }
            }
        //_______________________________________________________________________________________________________________________
        public static List<Test> getAllTests() {
            using (DataTable dt = GetItems("test")) {
                List<Test> list = new List<Test>();
                foreach (DataRow dr in dt.Rows) list.Add(getTestByID((long)dr["id"]));
                return list;
                }
            }
        //_______________________________________________________________________________________________________________________
        private static List<Group> getAllGroupsInBatch( long BatchId ) {
            List<Group> list = new List<Group>();
            DataTable dt = sqliteHelper.executeQuery("select distinct G.[id] from [groups] as G inner join [batchgroup] as BG ON G.[id] = BG.[groupid] where BG.[batchid] = @param1 order by BG.[order],BG.[id]", GetParameters(BatchId));
            foreach (DataRow dr in dt.Rows) list.Add(getGroupByID((long)dr["id"]));
            return list;
            }
        //_______________________________________________________________________________________________________________________
        private static List<Test> getAllTestsInGroup( long GroupId ) {
            List<Test> list = new List<Test>();
            DataTable dt = sqliteHelper.executeQuery("select distinct T.[id] from [test] as T inner join [grouptest] as GT ON T.[id] = GT.[testid] where GT.[groupid] = @param1 order by GT.[order],GT.[id]", GetParameters(GroupId));
            foreach (DataRow dr in dt.Rows) list.Add(getTestByID((long)dr["id"]));
            return list;
            }
        //_______________________________________________________________________________________________________________________
        private static List<OutputSet> getAllOutputSetsInTest(long TestId) {
            List<OutputSet> list = new List<OutputSet>();
            DataTable dt = sqliteHelper.executeQuery("select * from [outputset] where [id] in (select [outputsetid] from [testoutputset] where [testid] = @param1)", GetParameters(TestId));
            foreach (DataRow dr in dt.Rows)
                list.Add(new OutputSet((long)dr["id"], dr["name"].ToString(), dr["detail"].ToString(), dr["type"].ToString(), dr.IsNull("destmaid") ? null : getMAByID((long)dr["destmaid"])));
            return list;
            }
        //_______________________________________________________________________________________________________________________
        public static List<Config> getAllConfig() {
            using (DataTable dt = GetItems("config")) {
                List<Config> list = new List<Config>();
                foreach (DataRow dr in dt.Rows) list.Add(new Config((long)dr["id"], dr["name"].ToString(), dr["desc"]?.ToString(), dr["filecontent"].ToString()));
                return list;
                }
            }
        //_______________________________________________________________________________________________________________________
        public static List<Source> getAllSource() {
            using (DataTable dt = GetItems("source")) {
                List<Source> list = new List<Source>();
                foreach (DataRow dr in dt.Rows) list.Add(new Source((long)dr["id"], dr["name"].ToString(), dr["server"].ToString(), dr["user"].ToString(), dr["password"].ToString(), dr["authtype"].ToString()));
                return list;
                }
            }
        //_______________________________________________________________________________________________________________________
        public static List<ManagementAgentInfo> getAllMAgent() {
            using (DataTable dt = GetItems("managementagent")) {
                List<ManagementAgentInfo> list = new List<ManagementAgentInfo>();
                foreach (DataRow dr in dt.Rows)
                    list.Add(new ManagementAgentInfo((long)dr["id"], dr["name"].ToString(), dr["sourceid"].ToString() == "" ? null : getSourceByID((long)dr["sourceid"])));
                return list;
                }
            }
        //_______________________________________________________________________________________________________________________
        public static List<InputSet> getAllInputSet() {
            using (DataTable dt = GetItems("inputset")) {
                List<InputSet> list = new List<InputSet>();
                foreach (DataRow dr in dt.Rows) list.Add(new InputSet((long)dr["id"], dr["name"].ToString(), dr["detail"].ToString(), dr["dn"].ToString()));
                return list;
                }
            }
        //_______________________________________________________________________________________________________________________
        public static List<OutputSet> getAllOutputSet() {
            using (DataTable dt = GetItems("outputset")) {
                List<OutputSet> list = new List<OutputSet>();
                foreach (DataRow dr in dt.Rows)
                    list.Add(new OutputSet((long)dr["id"], dr["name"].ToString(), dr["detail"].ToString(), dr["type"].ToString(), dr.IsNull("destmaid") ? null : getMAByID((long)dr["destmaid"])));
                return list;
                }
            }
        //_______________________________________________________________________________________________________________________
        public static List<Script> getAllScript() {
            using (DataTable dt = GetItems("script")) {
                List<Script> list = new List<Script>();
                foreach (DataRow dr in dt.Rows) list.Add(new Script((long)dr["id"], dr["name"].ToString(), dr["desc"]?.ToString(), dr["filecontent"]?.ToString()));
                return list;
                }
            }
        //_______________________________________________________________________________________________________________________
        public static List<string> getAllHidden() {
            using (DataTable dt = GetItems("hiddenitem")) {
                List<string> list = new List<string>();
                foreach (DataRow dr in dt.Rows) list.Add(dr["name"].ToString());
                return list;
                }
            }
        //_______________________________________________________________________________________________________________________
        public static List<Location> getAllLocations() {
            using (DataTable dt = GetItems("location")) {
                List<Location> list = new List<Location>();
                foreach (DataRow dr in dt.Rows) list.Add(new Location((long)dr["id"], dr["name"].ToString(), dr["desc"].ToString(), getAllVariablesInLocation((long)dr["id"])));
                return list;
                }
            }
        //_______________________________________________________________________________________________________________________
        private static List<Variable> getAllVariablesInLocation(long LocationId) {
            List<Variable> list = new List<Variable>();
            string sql = @"SELECT [variable].id as id,[variable].name as name,[locationvariable].value as value,[variable].desc as desc FROM[variable] LEFT JOIN[locationvariable] ON[variable].id = [locationvariable].variableid WHERE[locationvariable].locationid = @param1";
            sql += " union ";
            sql += " select[variable].id,[variable].name,null,[variable].desc FROM[variable] where[variable].id not in (select[locationvariable].variableid from[locationvariable] where[locationvariable].locationid = @param1 )";
            DataTable dt = sqliteHelper.executeQuery(sql, GetParameters(LocationId));
            foreach (DataRow dr in dt.Rows)
                list.Add(new Variable((long)dr["id"], dr["name"].ToString(), dr["value"].ToString(), dr["desc"].ToString()));
            return list;
            }
        #endregion
        #region Batches
        //_______________________________________________________________________________________________________________________
        public static void saveBatch( long Id, string Name ) { if (!checkID("batch", Id)) createID("batch", Name); else updateID("batch", Id, Name); }
        //_______________________________________________________________________________________________________________________
        public static void deleteBatch( Batch batch ) { deleteID(batch.Table, batch.Id); deleteAllLinks("batchgroup", "batchid", batch.Id); deleteHiddenItem(batch.Link); }
        //_______________________________________________________________________________________________________________________
        public static void saveBatchGroup( Batch batch, Group group ) { addLink("batchgroup", batch.Table, "batchid", batch.Id, "groupid", group.Id); }
        //_______________________________________________________________________________________________________________________
        public static void deleteBatchGroup( Batch batch, Group group ) { deleteLink("batchgroup", "batchid", batch.Id, "groupid", group.Id); }
        //_______________________________________________________________________________________________________________________
        public static void copyBatch( Batch batch ) { duplicateRowInLnkTable("batchgroup", "batchid", batch.Id, duplicateRowInTable(batch.Table, batch.Id, batch.Name)); }
        //_______________________________________________________________________________________________________________________
        public static void setBatchGroupOrder( Batch batch , long GroupId, long order ) {
            string sSql = "update [batchgroup] set [order]=@param1 where [batchid]=@param2 and [groupid]=@param3";
            sqliteHelper.ExecuteNonQuery(sSql, GetParameters(order, batch.Id, GroupId));
            }
        #endregion
        #region Groups
        //_______________________________________________________________________________________________________________________
        public static void saveGroup(long Id, string Name) { if (!checkID("groups", Id)) createID("groups", Name); else updateID("groups", Id, Name); }
        //_______________________________________________________________________________________________________________________
        public static void deleteGroup(Group group) { deleteID(group.Table, group.Id); deleteAllLinks("grouptest", "groupid", group.Id); deleteHiddenItem(group.Link); }
        //_______________________________________________________________________________________________________________________
        public static void saveGroupTest( Group group, Test test ) { addLink("grouptest", group.Table, "groupid", group.Id, "testid", test.Id); }
        //_______________________________________________________________________________________________________________________
        public static void deleteGroupTest(Group group, Test test) { deleteLink("grouptest", "groupid", group.Id, "testid", test.Id); }
        //_______________________________________________________________________________________________________________________
        public static void copyGroup( Group group ) { duplicateRowInLnkTable("grouptest","groupid", group.Id, duplicateRowInTable(group.Table, group.Id, group.Name)); }
        //_______________________________________________________________________________________________________________________
        public static void setGroupTestOrder( Group group, long TestId, long order ) {
            string sSql = "update [grouptest] set [order]=@param1 where [groupid]=@param2 and [testid]=@param3";
            sqliteHelper.ExecuteNonQuery(sSql, GetParameters(order, group.Id, TestId));
            }
        #endregion
        #region Test
        //_______________________________________________________________________________________________________________________
        public static void saveTest( long Id, string Name ) { if (!checkID("test", Id)) createID("test", Name); else updateID("test", Id, Name); }
        //_______________________________________________________________________________________________________________________
        public static void saveTest( Test test, long Delta, long Commit ) { if (!checkID(test.Table, test.Id)) return; updateID(test.Table, test.Id, "delta", Delta); updateID(test.Table, test.Id, "commit", Commit); }
        //_______________________________________________________________________________________________________________________
        public static void saveTestDetail( Test test, string Detail ) { if (!checkID(test.Table, test.Id)) return; updateID(test.Table, test.Id, "desc", Detail); }
        //_______________________________________________________________________________________________________________________
        public static void saveTestConfig(Test test, Config obj) { if (!checkID(test.Table, test.Id)) return; updateID(test.Table, test.Id, "configid", obj.Id); }
        //_______________________________________________________________________________________________________________________
        public static void saveTestScript( Test test, Script obj ) { if (!checkID(test.Table, test.Id)) return; updateID(test.Table, test.Id, "scriptid", obj.Id); }
        //_______________________________________________________________________________________________________________________
        public static void saveTestInputSet(Test test, InputSet obj) { if (!checkID(test.Table, test.Id)) return; updateID(test.Table, test.Id, "inputsetid", obj.Id); }
        //_______________________________________________________________________________________________________________________
        public static void saveTestMA(Test test, ManagementAgentInfo obj) { if (!checkID(test.Table, test.Id)) return; updateID(test.Table, test.Id, "sourcemaid", obj.Id); }
        //_______________________________________________________________________________________________________________________
        public static void saveTestOutputSet(Test test, OutputSet obj) { addLink("testoutputset", test.Table, "testid", test.Id, "outputsetid", obj.Id); }
        //_______________________________________________________________________________________________________________________
        public static void deleteTest(Test test) {
            deleteID("test", test.Id);
            deleteAllLinks("grouptest", "testid", test.Id);
            deleteAllLinks("testoutputset", "testid", test.Id);
            deleteHiddenItem(test.Link);
            }
        //_______________________________________________________________________________________________________________________
        public static void deleteTestConfig(Test test) { updateID("test", test.Id, "configid", null); }
        //_______________________________________________________________________________________________________________________
        public static void deleteTestScript( Test test ) { updateID("test", test.Id, "scriptid", null); }
        //_______________________________________________________________________________________________________________________
        public static void deleteTestMA(Test test) { updateID("test", test.Id, "sourcemaid", null); }
        //_______________________________________________________________________________________________________________________
        public static void deleteTestInputSet(Test test) { updateID("test", test.Id, "inputsetid", null); }
        //_______________________________________________________________________________________________________________________
        public static void deleteTestOutputSet(Test test, OutputSet obj) { deleteLink("testoutputset", "testid",test.Id, "outputsetid", obj.Id); }
        //_______________________________________________________________________________________________________________________
        public static void copyTest( Test o ) { duplicateRowInLnkTable("testoutputset", "testid", o.Id, duplicateRowInTable("test", o.Id, o.Name)); }
        #endregion
        #region Config
       //_______________________________________________________________________________________________________________________
        public static void saveConfig(long Id, string Name) { if (!checkID("config", Id)) createID("config", Name); else updateID("config", Id, Name); }
        //_______________________________________________________________________________________________________________________
        public static void saveConfigDesc(Config config, string Desc) { if (!checkID(config.Table, config.Id)) return; updateID(config.Table, config.Id, "desc", Desc); }
        //_______________________________________________________________________________________________________________________
        public static void saveConfigDetail(Config config, string Detail) { if (!checkID(config.Table, config.Id)) return; updateID(config.Table, config.Id, "filecontent", Detail); }
        //_______________________________________________________________________________________________________________________
        public static void deleteConfig(Config config) { deleteID(config.Table, config.Id); deleteHiddenItem(config.Link); }
        //_______________________________________________________________________________________________________________________
        public static void copyConfig(Config config) { duplicateRowInTable(config.Table, config.Id, config.Name); }
        #endregion
        #region Source
        //_______________________________________________________________________________________________________________________
        public static void saveSource(long Id, string Name) { if (!checkID("source", Id)) createID("source", Name); else updateID("source", Id, Name); }
        //_______________________________________________________________________________________________________________________
        public static void saveSource( Source src, string Name, string Server, string User, string Pwd, string AuthType ) {
            if (!checkID(src.Table, src.Id)) return;
            updateID(src.Table, src.Id, "name", Name);
            updateID(src.Table, src.Id, "server", Server);
            updateID(src.Table, src.Id, "user", User);
            updateID(src.Table, src.Id, "password", Pwd);
            updateID(src.Table, src.Id, "authtype", AuthType);
            }
        //_______________________________________________________________________________________________________________________
        public static void deleteSource(Source src) { deleteID(src.Table, src.Id); deleteHiddenItem(src.Link); }
        //_______________________________________________________________________________________________________________________
        public static void copySource( Source src) { duplicateRowInTable(src.Table, src.Id, src.Name); }
        #endregion
        #region ManagementAgentInfo
        //_______________________________________________________________________________________________________________________
        public static void saveMA(long Id, string Name) { if (!checkID("managementagent", Id)) createID("managementagent", Name); else updateID("managementagent", Id, Name); }
        //_______________________________________________________________________________________________________________________
        public static void saveMA(ManagementAgentInfo oma, Source obj) { if (!checkID(oma.Table, oma.Id)) return; updateID(oma.Table, oma.Id, "sourceid", obj.Id); }
        //_______________________________________________________________________________________________________________________
        public static void deleteMA(ManagementAgentInfo oma) { deleteID(oma.Table, oma.Id); deleteHiddenItem(oma.Link); }
        //_______________________________________________________________________________________________________________________
        public static void deleteMASource( ManagementAgentInfo ma ) { updateID("managementagent", ma.Id, "sourceid", null); }
        //_______________________________________________________________________________________________________________________
        public static void copyMA( ManagementAgentInfo oma) { duplicateRowInTable(oma.Table, oma.Id, oma.Name); }
        #endregion
        #region InputSet
        //_______________________________________________________________________________________________________________________
        public static void saveInputSet(long Id, string Name) { if (!checkID("inputset", Id)) updateID("inputset", createID("inputset", Name), "detail", defaultInputDetail); else updateID("inputset", Id, Name); }
        //_______________________________________________________________________________________________________________________
        public static void saveInputSetDetail(InputSet inset, string Detail) { if (!checkID(inset.Table, inset.Id)) return; updateID(inset.Table, inset.Id, "detail", Detail); }
        //_______________________________________________________________________________________________________________________
        public static void saveInputSetDN( InputSet inset, string DN ) { if (!checkID(inset.Table, inset.Id)) return; updateID(inset.Table, inset.Id, "dn", DN); }
        //_______________________________________________________________________________________________________________________
        public static void deleteInputSet(InputSet inset) { deleteID(inset.Table, inset.Id); deleteHiddenItem(inset.Link); }
        //_______________________________________________________________________________________________________________________
        public static void copyInputSet(InputSet inset) { duplicateRowInTable(inset.Table, inset.Id, inset.Name); }
        #endregion
        #region OutputSet
        //_______________________________________________________________________________________________________________________
        public static void saveOutputSet(long Id, string Name) { if (!checkID("outputset", Id)) updateID("outputset", createID("outputset", Name), "detail", defaultOutputDetail); else updateID("outputset", Id, Name); }
         //_______________________________________________________________________________________________________________________
        public static void saveOutputSetDetail(OutputSet oset, string Detail) { if (!checkID(oset.Table, oset.Id)) return; updateID(oset.Table, oset.Id, "detail", Detail); }
        //_______________________________________________________________________________________________________________________
        public static void saveOutputSetType( OutputSet oset, string Type) {
            if (!checkID(oset.Table, oset.Id)) return;
            updateID(oset.Table, oset.Id, "type", Type);
            oset.Type = Type;
            if (!oset.MARequired) saveOutputSetMA(oset, null);
            if (!oset.DetailRequired) saveOutputSetDetail(oset, null);
            if (oset.DetailRequired && string.IsNullOrWhiteSpace(oset.Detail)) saveOutputSetDetail(oset, defaultOutputDetail);
            }
       //_______________________________________________________________________________________________________________________
        public static void saveOutputSetMA(OutputSet oset, ManagementAgentInfo obj) {
            if (!checkID(oset.Table, oset.Id)) return;
            if (obj != null) updateID(oset.Table, oset.Id, "destmaid", obj.Id);
            else updateID(oset.Table, oset.Id, "destmaid", null);
            }
        //_______________________________________________________________________________________________________________________
        public static void deleteOutputSet(OutputSet oset) { deleteID(oset.Table, oset.Id); deleteAllLinks("testoutputset", "outputsetid", oset.Id); deleteHiddenItem(oset.Link); }
        //_______________________________________________________________________________________________________________________
        public static void deleteOutputSetMA(OutputSet oset) { updateID(oset.Table, oset.Id, "destmaid", null); }
        //_______________________________________________________________________________________________________________________
        public static void copyOutputSet( OutputSet oset) { duplicateRowInTable(oset.Table, oset.Id, oset.Name); }
        #endregion
        #region Script
        //_______________________________________________________________________________________________________________________
        public static void saveScript( long Id, string Name ) { if (!checkID("script", Id)) updateID("script", createID("script", Name), "filecontent", defaultScript); else updateID("script", Id, Name); }
        //_______________________________________________________________________________________________________________________
        public static void saveScriptDesc( Script script, string Desc ) { if (!checkID(script.Table, script.Id)) return; updateID(script.Table, script.Id, "desc", Desc); }
        //_______________________________________________________________________________________________________________________
        public static void saveScriptDetail( Script script, string Detail ) { if (!checkID(script.Table, script.Id)) return; updateID(script.Table, script.Id, "filecontent", Detail); }
        //_______________________________________________________________________________________________________________________
        public static void deleteScript( Script script ) { deleteID(script.Table, script.Id); deleteHiddenItem(script.Link); }
        //_______________________________________________________________________________________________________________________
        public static void copyScript( Script script ) { duplicateRowInTable(script.Table, script.Id, script.Name); }
        #endregion
        #region location
        //_______________________________________________________________________________________________________________________
        public static long saveLocation(long Id, string Name) { if (!checkID("location", Id)) return createID("location", Name); else return updateID("location", Id, Name); }
        //_______________________________________________________________________________________________________________________
        public static void saveLocationDetail(Location location, string Detail) { if (!checkID(location.Table, location.Id)) return; updateID(location.Table, location.Id, "desc", Detail); }
        //_______________________________________________________________________________________________________________________
        public static void saveLocationVariable(Location location, Variable obj) { addLink("locationvariable", location.Table, "locationid", location.Id, "variableid", obj.Id); }
        //_______________________________________________________________________________________________________________________
        public static void setLocationVariableValue(Location location, long variableId, string value) {
            string sSql = "update [locationvariable] set [value]=@param1 where [locationid]=@param2 and [variableid]=@param3";
            sqliteHelper.ExecuteNonQuery(sSql, GetParameters(value, location.Id, variableId));
            }
        //_______________________________________________________________________________________________________________________
        public static void deleteLocation(Location location) { deleteID(location.Table, location.Id); deleteAllLinks("locationvariable", "locationid", location.Id); deleteHiddenItem(location.Link); }
        #endregion
        #region variable
        //_______________________________________________________________________________________________________________________
        public static long saveVariable(long Id, string Name) { if (!checkID("variable", Id)) return createID("variable", Name); else return updateID("variable", Id, Name); }
        //_______________________________________________________________________________________________________________________
        public static void saveVariableDetail(Variable variable, string Detail) { if (!checkID(variable.Table, variable.Id)) return; updateID(variable.Table, variable.Id, "desc", Detail); }
        //_______________________________________________________________________________________________________________________
        public static void deleteVariable(Variable variable) { deleteID(variable.Table, variable.Id); deleteAllLinks("locationvariable", "variableid", variable.Id); deleteHiddenItem(variable.Link); }
        #endregion
        #region Hidden
        //_______________________________________________________________________________________________________________________
        //public static void saveHiddenItem(long Id, string Name) { if (!checkID("hiddenitem", Id)) createID("hiddenitem", Name); else updateID("hiddenitem", Id, Name); }
        public static bool saveHiddenItem(object sLnk) {
            var o = TestObjectBase.LinkedObject(sLnk);
            if (o == null) return false;
            insertByName("hiddenitem", o.Link);
            //TestsHelper.saveHiddenItem(TestObjectBase.NO_ID, o.Link);
            return true;
            }
        //_______________________________________________________________________________________________________________________
        public static void deleteHiddenItem(string Name) { deleteByName("hiddenitem", Name); }
        public static bool deleteHiddenItem(object sLnk) {
            TestObjectBase o = TestObjectBase.LinkedObject(sLnk);
            if (o == null) return false;
            TestsHelper.deleteHiddenItem(o.Link);
            return true;
            }
        #endregion
        #region Clean Links
        //_______________________________________________________________________________________________________________________
        public static void cleanBrokenLinks() { cleanBrokenBatchGroupLinks(); cleanBrokenGroupTestLinks(); cleanBrokenTestOutputSetLinks(); }
        //_______________________________________________________________________________________________________________________
        private static void cleanBrokenBatchGroupLinks() { cleanLnkTable("batchgroup", "batch", "batchid"); cleanLnkTable("batchgroup", "groups", "groupid"); }
        //_______________________________________________________________________________________________________________________
        private static void cleanBrokenGroupTestLinks() { cleanLnkTable("grouptest", "groups", "groupid"); cleanLnkTable("grouptest", "test", "testid"); }
        //_______________________________________________________________________________________________________________________
        private static void cleanBrokenTestOutputSetLinks() { cleanLnkTable("testoutputset", "outputset", "outputsetid"); cleanLnkTable("testoutputset", "test", "testid"); }
        #endregion
        #region Public Utilities
        //_______________________________________________________________________________________________________________________
        public static long MaxID(string sTabla) { try { return 1 + (long)sqliteHelper.ExecuteScalar("select max([ID]) from [" + sTabla + "]"); } catch (Exception) { return 1; } }
        //_______________________________________________________________________________________________________________________
        public static string[] getOutputSetTypes() { return Enum.GetNames(typeof(OutputSet.AllowedTypes)); }
        //_______________________________________________________________________________________________________________________
        public static string GetTableName(string Name) {
            switch (Name) {
                case "tvBatches": case "bBatches": return "batch";
                case "tvGroups": case "bGroups": return "groups";
                case "tvTests": case "bTests": return "test";
                case "tvConfigs": case "bConfigs": return "config";
                case "tvMAs": case "bMAs": return "managementagent";
                case "tvSources": case "bSources": return "source";
                case "tvInputSets": case "bInputSets": return "inputset";
                case "tvOutputSets": case "bOutputSets": return "outputset";
                case "tvScripts": case "bScripts": return "script";
                }
            return "";
            }
        //_______________________________________________________________________________________________________________________
        public static bool IsValidDrag(Type torg, Type tdst) {
            if (torg == null || tdst == null) return false;
            if (torg.Equals(tdst)) return false;
            if (torg.Equals(typeof(Group)) && tdst.Equals(typeof(Batch))) return true;
            if (torg.Equals(typeof(Test)) && tdst.Equals(typeof(Group))) return true;
            if (torg.Equals(typeof(Config)) && tdst.Equals(typeof(Test))) return true;
            if (torg.Equals(typeof(Script)) && tdst.Equals(typeof(Test))) return true;
            if (torg.Equals(typeof(InputSet)) && tdst.Equals(typeof(Test))) return true;
            if (torg.Equals(typeof(OutputSet)) && tdst.Equals(typeof(Test))) return true;
            if (torg.Equals(typeof(ManagementAgentInfo)) && tdst.Equals(typeof(Test))) return true;
            if (torg.Equals(typeof(ManagementAgentInfo)) && tdst.Equals(typeof(OutputSet))) return true;
            if (torg.Equals(typeof(Source)) && tdst.Equals(typeof(ManagementAgentInfo))) return true;
            return false;
            }
        //_______________________________________________________________________________________________________________________
        public static bool CompleteDrop(TestObjectBase org, TestObjectBase dst) {
            if (!IsValidDrag(org.GetType(), dst.GetType())) return false;

            if (org.GetType().Equals(dst.GetType())) return false;
            if (org.GetType().Equals(typeof(Group)) && dst.GetType().Equals(typeof(Batch))) TestsHelper.saveBatchGroup((Batch)dst, (Group)org);
            if (org.GetType().Equals(typeof(Test)) && dst.GetType().Equals(typeof(Group))) TestsHelper.saveGroupTest((Group)dst, (Test)org);
            if (org.GetType().Equals(typeof(Config)) && dst.GetType().Equals(typeof(Test))) TestsHelper.saveTestConfig((Test)dst, (Config)org);
            if (org.GetType().Equals(typeof(Script)) && dst.GetType().Equals(typeof(Test))) TestsHelper.saveTestScript((Test)dst, (Script)org);
            if (org.GetType().Equals(typeof(InputSet)) && dst.GetType().Equals(typeof(Test))) TestsHelper.saveTestInputSet((Test)dst, (InputSet)org);
            if (org.GetType().Equals(typeof(OutputSet)) && dst.GetType().Equals(typeof(Test))) TestsHelper.saveTestOutputSet((Test)dst, (OutputSet)org);
            if (org.GetType().Equals(typeof(ManagementAgentInfo)) && dst.GetType().Equals(typeof(Test))) TestsHelper.saveTestMA((Test)dst, (ManagementAgentInfo)org);
            if (org.GetType().Equals(typeof(ManagementAgentInfo)) && dst.GetType().Equals(typeof(OutputSet))) TestsHelper.saveOutputSetMA((OutputSet)dst, (ManagementAgentInfo)org);
            if (org.GetType().Equals(typeof(Source)) && dst.GetType().Equals(typeof(ManagementAgentInfo))) TestsHelper.saveMA((ManagementAgentInfo)dst, (Source)org);
            return true;
            }
        //_______________________________________________________________________________________________________________________
        public static void UpdateName(TestObjectBase o, string newName) {
            if (o == null || string.IsNullOrWhiteSpace(newName)) return;

            if (o.GetType().Equals(typeof(Batch))) TestsHelper.saveBatch(((Batch)o).Id, newName);
            if (o.GetType().Equals(typeof(Group))) TestsHelper.saveGroup(((Group)o).Id, newName);
            if (o.GetType().Equals(typeof(Test))) TestsHelper.saveTest(((Test)o).Id, newName);
            if (o.GetType().Equals(typeof(Config))) TestsHelper.saveConfig(((Config)o).Id, newName);
            if (o.GetType().Equals(typeof(ManagementAgentInfo))) TestsHelper.saveMA(((ManagementAgentInfo)o).Id, newName);
            if (o.GetType().Equals(typeof(Source))) TestsHelper.saveSource(((Source)o).Id, newName);
            if (o.GetType().Equals(typeof(InputSet))) TestsHelper.saveInputSet(((InputSet)o).Id, newName);
            if (o.GetType().Equals(typeof(OutputSet))) TestsHelper.saveOutputSet(((OutputSet)o).Id, newName);
            if (o.GetType().Equals(typeof(Script))) TestsHelper.saveScript(((Script)o).Id, newName);
            }
        //_______________________________________________________________________________________________________________________
        public static bool SaveObjectDetail(TestObjectBase o, string value) {
            if (o == null) return false;
            if (o.GetType().Equals(typeof(Test))) TestsHelper.saveTestDetail((Test)o, value);
            if (o.GetType().Equals(typeof(Config))) TestsHelper.saveConfigDetail((Config)o, value);
            if (o.GetType().Equals(typeof(InputSet))) TestsHelper.saveInputSetDetail((InputSet)o, value);
            if (o.GetType().Equals(typeof(OutputSet))) TestsHelper.saveOutputSetDetail((OutputSet)o, value);
            if (o.GetType().Equals(typeof(Script))) TestsHelper.saveScriptDetail((Script)o, value);
            return true;
            }
        //_______________________________________________________________________________________________________________________
        public static bool Delete(object sLnk) {
            TestObjectBase o = TestObjectBase.LinkedObject(sLnk);
            if (o == null) return false;
            if (o.GetType().Equals(typeof(Batch))) TestsHelper.deleteBatch((Batch)o);
            if (o.GetType().Equals(typeof(Group))) TestsHelper.deleteGroup((Group)o);
            if (o.GetType().Equals(typeof(Test))) TestsHelper.deleteTest((Test)o);
            if (o.GetType().Equals(typeof(Config))) TestsHelper.deleteConfig((Config)o);
            if (o.GetType().Equals(typeof(ManagementAgentInfo))) TestsHelper.deleteMA((ManagementAgentInfo)o);
            if (o.GetType().Equals(typeof(Source))) TestsHelper.deleteSource((Source)o);
            if (o.GetType().Equals(typeof(InputSet))) TestsHelper.deleteInputSet((InputSet)o);
            if (o.GetType().Equals(typeof(OutputSet))) TestsHelper.deleteOutputSet((OutputSet)o);
            if (o.GetType().Equals(typeof(Script))) TestsHelper.deleteScript((Script)o);
            return true;
            }
        //_______________________________________________________________________________________________________________________
        public static bool DeleteSubItem(object[] os) {
            if (os == null) return false;
            if (os[0].GetType().Equals(typeof(Batch))) TestsHelper.deleteBatchGroup((Batch)os[0], (Group)os[1]);
            if (os[0].GetType().Equals(typeof(Group))) TestsHelper.deleteGroupTest((Group)os[0], (Test)os[1]);
            if (os[0].GetType().Equals(typeof(ManagementAgentInfo))) TestsHelper.deleteMASource((ManagementAgentInfo)os[0]);
            if (os[0].GetType().Equals(typeof(Test))) {
                if (os[1].GetType().Equals(typeof(Config))) TestsHelper.deleteTestConfig((Test)os[0]);
                if (os[1].GetType().Equals(typeof(ManagementAgentInfo))) TestsHelper.deleteTestMA((Test)os[0]);
                if (os[1].GetType().Equals(typeof(InputSet))) TestsHelper.deleteTestInputSet((Test)os[0]);
                if (os[1].GetType().Equals(typeof(OutputSet))) TestsHelper.deleteTestOutputSet((Test)os[0], (OutputSet)os[1]);
                if (os[1].GetType().Equals(typeof(Script))) TestsHelper.deleteTestScript((Test)os[0]);
                }
            if (os[0].GetType().Equals(typeof(OutputSet))) TestsHelper.deleteOutputSetMA((OutputSet)os[0]);

            return true;
            }
        //_______________________________________________________________________________________________________________________
        public static bool Add(string type) {
            if (string.IsNullOrEmpty(type)) return false;
            string tableName = TestsHelper.GetTableName(type);
            string newName = "new " + (type+" ").Substring(2).Replace("es ","").Replace("s ","").Trim() + " " + TestsHelper.MaxID(tableName);

            if (type == "tvBatches") TestsHelper.saveBatch(TestObjectBase.NO_ID, newName);
            if (type == "tvGroups") TestsHelper.saveGroup(TestObjectBase.NO_ID, newName);
            if (type == "tvTests") TestsHelper.saveTest(TestObjectBase.NO_ID, newName);
            if (type == "tvConfigs") TestsHelper.saveConfig(TestObjectBase.NO_ID, newName + ".xml");
            if (type == "tvMAs") TestsHelper.saveMA(TestObjectBase.NO_ID, newName);
            if (type == "tvSources") TestsHelper.saveSource(TestObjectBase.NO_ID, newName);
            if (type == "tvInputSets") TestsHelper.saveInputSet(TestObjectBase.NO_ID, newName);
            if (type == "tvOutputSets") TestsHelper.saveOutputSet(TestObjectBase.NO_ID, newName);
            if (type == "tvScripts") TestsHelper.saveScript(TestObjectBase.NO_ID, newName + ".ps1");
            return true;
            }
        //_______________________________________________________________________________________________________________________
        public static bool Copy(object sLnk) {
            object o = TestObjectBase.LinkedObject(sLnk);
            if (o == null) return false;
            if (o.GetType().Equals(typeof(Batch))) TestsHelper.copyBatch((Batch)o);
            if (o.GetType().Equals(typeof(Group))) TestsHelper.copyGroup((Group)o);
            if (o.GetType().Equals(typeof(Test))) TestsHelper.copyTest((Test)o);
            if (o.GetType().Equals(typeof(Config))) TestsHelper.copyConfig((Config)o);
            if (o.GetType().Equals(typeof(ManagementAgentInfo))) TestsHelper.copyMA((ManagementAgentInfo)o);
            if (o.GetType().Equals(typeof(Source))) TestsHelper.copySource((Source)o);
            if (o.GetType().Equals(typeof(InputSet))) TestsHelper.copyInputSet((InputSet)o);
            if (o.GetType().Equals(typeof(OutputSet))) TestsHelper.copyOutputSet((OutputSet)o);
            if (o.GetType().Equals(typeof(Script))) TestsHelper.copyScript((Script)o);
            return true;
            }
        //_______________________________________________________________________________________________________________________
        public static bool Detail(object sLnk, out string value, out TestObjectBase o) {
            o = TestObjectBase.LinkedObject(sLnk);
            value = "";
            if (o == null) return false;
            if (o.GetType().Equals(typeof(Test))) { value = ((Test)o).Detail.Replace("\r\n", "\n").Replace("\n", "\r\n"); return true; }
            if (o.GetType().Equals(typeof(Config))){ value = ((Config)o).FileContent.Replace("\r\n", "\n").Replace("\n", "\r\n"); return true; }
            if (o.GetType().Equals(typeof(InputSet))) { value = ((InputSet)o).Detail.Replace("\r\n", "\n").Replace("\n", "\r\n"); return true; }
            if (o.GetType().Equals(typeof(OutputSet))) { value = ((OutputSet)o).Detail.Replace("\r\n", "\n").Replace("\n", "\r\n"); return true; }
            if (o.GetType().Equals(typeof(Script))) { value = ((Script)o).FileContent.Replace("\r\n", "\n").Replace("\n", "\r\n"); return true; }
            return false;
            }
        //_______________________________________________________________________________________________________________________
        public static void LaunchTool(string sTool,string DBFile) {
            if (string.IsNullOrEmpty(sTool)) return;
            switch (sTool) {
                case "FIMTestRunnerApp": Utilities.LaunchApp("FIMTestRunnerApp", "Test Editor", DBFile); break;
                case "FIMSyncDivApp": Utilities.LaunchApp("FIMSyncDivApp", "FIM Sync Div"); break;
                case "FIMSyncNatApp": Utilities.LaunchApp("FIMSyncNatApp", "FIM Sync Nat"); break;
                case "FIMConfigFiles":
                    if (string.IsNullOrWhiteSpace(ConfigurationHelper.GetSetting("FIMConfigFiles")))
                        ConfigurationHelper.SetSetting("FIMConfigFiles", @" -multiInst -nosession ""D:\FIMSynchronization\Synchronization Service\Extensions\config*.xml""  ""D:\FIMSynchronization\Synchronization Service\Extensions\configurationsToLoad.txt""");
                    Utilities.LaunchApp("Notepad++", "Notepad++", ConfigurationHelper.GetSetting("FIMConfigFiles"));
                    break;
                }
            }
        #endregion
        #region Private Utilities
        //_______________________________________________________________________________________________________________________
        private static DataTable GetItems(string sTabla, long Id = TestObjectBase.NO_ID) { return sqliteHelper.executeQuery("select * from [" + sTabla + "]" + (Id == TestObjectBase.NO_ID ? "" : " where [id] = @param1"), GetParameters(Id)); }
        //_______________________________________________________________________________________________________________________
        private static bool checkID(string sTabla, long Id) {
            if (Id == TestObjectBase.NO_ID) return false;
            return (1 == (long)sqliteHelper.ExecuteScalar("select count(*) from [" + sTabla + "] where [id]=@param1", GetParameters(Id)));
            }
        //_______________________________________________________________________________________________________________________
        private static void deleteID(string sTabla, long Id) {
            if (Id == TestObjectBase.NO_ID) return;
            sqliteHelper.ExecuteNonQuery("delete from [" + sTabla + "] where [id]=@param1", GetParameters(Id));
            }
        //_______________________________________________________________________________________________________________________
        private static void deleteByName(string sTabla, string Name) {
            if (string.IsNullOrWhiteSpace(Name)) return;
            sqliteHelper.ExecuteNonQuery("delete from [" + sTabla + "] where [name]=@param1", GetParameters(Name));
            }
        //_______________________________________________________________________________________________________________________
        private static void insertByName(string sTabla, string Name) {
            if (string.IsNullOrWhiteSpace(Name)) return;
            long Id = MaxID(sTabla);
            sqliteHelper.ExecuteNonQuery("INSERT INTO [" + sTabla + "] ([Id], [Name]) SELECT @param1, @param2 WHERE NOT EXISTS (SELECT 1 FROM [" + sTabla + "] WHERE [Name] = @param2)", GetParameters(Id, Name));
            }
        //_______________________________________________________________________________________________________________________
        private static void addLink( string sTabla, string sTablaPadre, string sColumnaPadre, long IdPadre, string sColumnaHijo, long IdHijo ) {
            if (!checkID(sTablaPadre, IdPadre)) return;
            sqliteHelper.ExecuteNonQuery("INSERT INTO [" + sTabla + "] ([" + sColumnaPadre + "], [" + sColumnaHijo + "]) SELECT @param1, @param2 WHERE NOT EXISTS (SELECT 1 FROM [" + sTabla + "] WHERE [" + sColumnaPadre + "] = @param1 AND [" + sColumnaHijo + "] = @param2)", GetParameters(IdPadre, IdHijo));
            }
        //_______________________________________________________________________________________________________________________
        private static void deleteLink(string sTabla, string sColumnaPadre, long IdPadre, string sColumnaHijo, long IdHijo) {
            sqliteHelper.ExecuteNonQuery("delete from [" + sTabla + "] where [" + sColumnaPadre + "]=@param1 and [" + sColumnaHijo + "]=@param2", GetParameters(IdPadre, IdHijo));
            }
        //_______________________________________________________________________________________________________________________
        private static void deleteAllLinks(string sTabla, string sColumna, long Id) {
            sqliteHelper.ExecuteNonQuery("delete from [" + sTabla + "] where [" + sColumna + "]=@param1", GetParameters(Id));
            }
        //_______________________________________________________________________________________________________________________
        private static long createID(string sTabla, string Name) {
            long Id = MaxID(sTabla);
            sqliteHelper.ExecuteNonQuery("insert into [" + sTabla + "] ([id], [name]) VALUES (@param1, @param2)", GetParameters(Id, Name));
            return Id;
            }
        //_______________________________________________________________________________________________________________________
        private static long updateID(string sTabla, long Id, string Name) {
            sqliteHelper.ExecuteNonQuery("update [" + sTabla + "] set [name] = @param2 where ([id]=@param1)", GetParameters(Id, Name));
            return Id;
            }
        //_______________________________________________________________________________________________________________________
        private static void updateID(string sTabla, long Id, string sColumna, long Valor) {
            sqliteHelper.ExecuteNonQuery("update [" + sTabla + "] set ["+ sColumna + "] = @param2 where ([id]=@param1)", GetParameters(Id, Valor));
            }
        //_______________________________________________________________________________________________________________________
        private static void updateID(string sTabla, long Id, string sColumna, string Valor) {
            sqliteHelper.ExecuteNonQuery("update [" + sTabla + "] set [" + sColumna + "] = @param2 where ([id]=@param1)", GetParameters(Id, (object)Valor ?? DBNull.Value));
            }
        //_______________________________________________________________________________________________________________________
        private static SQLiteParameter[] GetParameters(object param1, object param2 = null, object param3 = null ) {
            SQLiteParameter[] parameters = new SQLiteParameter[1 + (param2 == null ? 0 : 1) + (param3 == null ? 0 : 1)];
            parameters[0] = new SQLiteParameter("@param1", param1);
            if (param2 != null) parameters[1] = new SQLiteParameter("@param2", param2);
            if (param2 != null && param3 != null) parameters[2] = new SQLiteParameter("@param3", param3);
            return parameters;
            }
        //_______________________________________________________________________________________________________________________
        private static void duplicateRowInLnkTable( string sTabla, string sRefCol, long Id, long NewId) {
            SQLiteParameter[] parameters = new SQLiteParameter[2];
            string Sql = @"CREATE TEMPORARY TABLE [tmp] AS SELECT * FROM [#TABLA] WHERE [#REFCOL]=@Id;
            UPDATE [tmp] SET [id] = NULL;
            UPDATE [tmp] SET [#REFCOL] = @newid;
            INSERT INTO [#TABLA] SELECT * FROM [tmp];
            DROP TABLE tmp;".Replace("#TABLA", sTabla).Replace("#REFCOL", sRefCol);
            parameters[0] = new SQLiteParameter("@id", Id);
            parameters[1] = new SQLiteParameter("@newid", NewId);
            sqliteHelper.ExecuteNonQuery(Sql, parameters);
            }
        //_______________________________________________________________________________________________________________________
        private static long duplicateRowInTable( string sTabla, long Id, string Name ) {
            long newId = MaxID(sTabla);
            Name += " ("+ newId + ")";
            SQLiteParameter[] parameters = new SQLiteParameter[3];
            string Sql = @"CREATE TEMPORARY TABLE [tmp] AS SELECT * FROM [#TABLA] WHERE [Id]=@id;
            UPDATE [tmp] SET [id]= @newid;
            UPDATE [tmp] SET [Name] = @name;
            INSERT INTO [#TABLA] SELECT * FROM [tmp];
            DROP TABLE [tmp];".Replace("#TABLA", sTabla);
            parameters[0] = new SQLiteParameter("@id", Id);
            parameters[1] = new SQLiteParameter("@name", Name);
            parameters[2] = new SQLiteParameter("@newid", newId);
            sqliteHelper.ExecuteNonQuery(Sql, parameters);
            return newId;
            }
        //_______________________________________________________________________________________________________________________
        private static void cleanLnkTable( string sTabla, string sLnkTabla, string sLnkCol) {
            string Sql = "DELETE FROM [" + sTabla + "] WHERE ["+ sLnkCol + "] NOT IN (SELECT [ID] FROM ["+ sLnkTabla + "])";
            sqliteHelper.ExecuteNonQuery(Sql);
            }
        #endregion
        }
    }
