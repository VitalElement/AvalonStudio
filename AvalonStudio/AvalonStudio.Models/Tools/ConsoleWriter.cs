using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

//namespace VEStudio.Models.Tools
//{
//    public class ConsoleWriter : IConsole
//    {
//        private IConsole console;
//        private Dispatcher dispatcher;
//        public ConsoleWriter (IConsole console)
//        {
//            dispatcher = Dispatcher.CurrentDispatcher;
//            this.console = console;
//        }

//   //     public override void Write (char value)
//   //     {
//   //         base.Write (value);
//   //         console.Write (value);
//   //     }

//   //     public override void WriteLine()
//   //     {
//   //         WriteLine(string.Empty);
//   //     }

//   //     public override void WriteLine (string value)
//   //     {
//			////dispatcher.Invoke (() =>
//			//{
//   //             console.WriteLine(value);
//			//	//base.WriteLine (value);
//			//}//);
//   //     }

//   //     public override void Write (string value)
//   //     {
//   //         //dispatcher.Invoke (() =>
//   //         {
//   //             base.Write (value);
//   //         }//);
//   //     }

//        public void Clear()
//        {
//            this.console.Clear ();
//        //}

//        //public override Encoding Encoding
//        //{
//        //    get { return Encoding.UTF8; }
//        //}
//    }

//        public void Write(char data)
//        {
//            throw new NotImplementedException();
//        }

//        public void WriteLine(string data)
//        {
//            throw new NotImplementedException();
//        }
//    }
