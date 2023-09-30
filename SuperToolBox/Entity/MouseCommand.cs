using SuperToolBox.Config;
using SuperUtils.Enums;
using SuperUtils.Framework.ORM.Attributes;
using SuperUtils.Framework.ORM.Enums;
using SuperUtils.Framework.ORM.Mapper;
using SuperUtils.WPF.VieModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SuperUtils.Framework.Hooks.HookManager;

namespace SuperToolBox.Entity
{
    [Table(tableName: "mouse_command")]
    public class MouseCommand : ViewModelBase
    {
        public static int DEFAULT_DELAY = 1000;
        public static int DEFAULT_ORDER = 1;
        private long _ID;

        [TableId(IdType.AUTO)]
        public long ID {
            get { return _ID; }
            set { _ID = value; RaisePropertyChanged(); }
        }
        private int _PointX;
        public int PointX {
            get { return _PointX; }
            set {
                _PointX = value;
                MousePointText = $"({value},{PointY})";
                RaisePropertyChanged();
            }
        }
        private int _PointY;
        public int PointY {
            get { return _PointY; }
            set {
                _PointY = value;
                MousePointText = $"({PointX},{value})";
                RaisePropertyChanged();
            }
        }

        private MouseButton _MouseButton;
        public MouseButton MouseButton {
            get { return _MouseButton; }
            set {
                _MouseButton = value;
                RaisePropertyChanged();
            }
        }
        private MouseAction _MouseAction;
        public MouseAction MouseAction {
            get { return _MouseAction; }
            set {
                _MouseAction = value;
                RaisePropertyChanged();
            }
        }

        private int _Delay;
        public int Delay {
            get { return _Delay; }
            set {
                _Delay = value;
                RaisePropertyChanged();
            }
        }
        private int _CommandOrder;

        public int CommandOrder {
            get { return _CommandOrder; }
            set {
                _CommandOrder = value;
                RaisePropertyChanged();
            }
        }

        private RunningStatus _Status = RunningStatus.Waiting;

        [TableField(exist: false)]
        public RunningStatus Status {
            get { return _Status; }
            set {
                _Status = value;
                RaisePropertyChanged();
            }
        }
        private string _MousePointText = "(0,0)";

        [TableField(exist: false)]
        public string MousePointText {
            get { return _MousePointText; }
            set {
                _MousePointText = value;
                RaisePropertyChanged();
            }
        }


        // 必须要有无参构造器
        public MouseCommand()
        {

        }

        public static class SqliteTable
        {
            public static Dictionary<string, string> Table = new Dictionary<string, string>()
            {
                {"mouse_command","create table if not exists mouse_command( ID INTEGER PRIMARY KEY autoincrement, PointX INTEGER,PointY INTEGER, MouseButton INTEGER,MouseAction INTEGER,Delay INTEGER,CommandOrder INTEGER, CreateDate VARCHAR(30) DEFAULT(STRFTIME('%Y-%m-%d %H:%M:%S', 'NOW', 'localtime')), UpdateDate VARCHAR(30) DEFAULT(STRFTIME('%Y-%m-%d %H:%M:%S', 'NOW', 'localtime')) );" }
            };

        }

        public static void InitSqlite()
        {
            SqliteMapper<MouseCommand> mapper = new SqliteMapper<MouseCommand>(ConfigManager.SQLITE_DATA_PATH);
            foreach (var item in SqliteTable.Table.Keys) {
                if (!mapper.IsTableExists(item)) {
                    mapper.CreateTable(item, SqliteTable.Table[item]);
                }
            }
        }

        public override void Init()
        {
            throw new NotImplementedException();
        }
    }


}
