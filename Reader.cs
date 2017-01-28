using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WurmMesi
{
    /// <summary>
    /// ログ読込み
    /// 参照しているテキストに書込みが発生したらReadLineRecievedイベントを発生させる
    /// </summary>
    public class Reader : IDisposable
    {
        FileStream fileStream;
        StreamReader streamReader;
        Task task;
        bool isPower = true;

        public event EventHandler<ReadLineEventArgs> OnReadLineRecieved;

        /// <summary>
        /// コンストラクタ処理
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="encoding"></param>
        public Reader(string filename, Encoding encoding)
        {
            fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            fileStream.Seek(0, SeekOrigin.End);
            streamReader = new StreamReader(fileStream, encoding);
            task = Task.Factory.StartNew(TaskMain);
        }


        /// <summary>
        /// タスクメイン
        /// </summary>
        void TaskMain()
        {
            while (isPower)
            {
                while (!streamReader.EndOfStream && isPower)
                {
                    var line = streamReader.ReadLine();

                    if (null != OnReadLineRecieved)
                    {
                        OnReadLineRecieved?.Invoke(this, new ReadLineEventArgs(line));
                    }
                }

                Thread.Sleep(100);
            }
        }


        /// <summary>
        /// 開放処理
        /// </summary>
        public void Dispose()
        {
            isPower = false;
            task.Wait();
            streamReader.Dispose();
            fileStream.Dispose();
        }
    }


    public class ReadLineEventArgs : EventArgs
    {
        public string Line { get; }

        public ReadLineEventArgs(string line)
        {
            Line = line;
        }
    }
}
