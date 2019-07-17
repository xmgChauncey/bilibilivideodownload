/*
 * Created by SharpDevelop.
 * User: xmg
 * Date: 2019/5/7
 * Time: 9:39
 * Function:You can use this program to download the bilibili videos.
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace bilibilivideodownload
{
	class Program
	{
		public static void Main(string[] args)
		{
            // TODO: Implement Functionality Here
            //string filePath=args[0].ToString();
            string filePath = @"E:\bilibiivideo\aspnet.txt";

            FileStream fs = new FileStream(filePath,FileMode.Open,FileAccess.Read);
			StreamReader streamReader=new StreamReader(fs);
			streamReader.BaseStream.Seek(0,SeekOrigin.Begin);
			
			string[] paramArray;
			string strLine=streamReader.ReadLine();
			
			while(strLine!=null)
			{			
				paramArray = strLine.Split(',');
				if(paramArray.Length==4)
				{
					Thread myThread=new Thread(new ParameterizedThreadStart(GetVideo));
					object strline=strLine;
					myThread.Start(strline);
				}
				strLine=streamReader.ReadLine();
			}
			
			streamReader.Close();
			fs.Close();
			
			Console.Write("Wait for downloading finished . . . ");
			Console.ReadKey(true);
		}
		
		static void GetVideo(object strLine)
		{					
			Process myProcess= new Process();
			
			string[] paramArray;
			paramArray=strLine.ToString().Split(',');
			string yougetPath=paramArray[0].ToString();
			string videoDirectory=paramArray[1].ToString();
			string videourl=paramArray[2].ToString();
			int videoseries=Int32.Parse(paramArray[3].ToString());

            //创建日志文件
            string logFilePath = videoDirectory + @"\log.txt";
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath);
            }

            string arguments="";
            string logString = "";
			
			ProcessStartInfo myProcessStartInfo;         

            for (int i=1;i<=videoseries;i++)
			{
				string videourldownload=videourl+ @"/?p="+i;
				arguments="-o "+videoDirectory+" "+videourldownload;
				logString=string.Format("you-get {0}/{1} {2}", arguments, videoseries, DateTime.Now);
                StreamWriter streamWriter = new StreamWriter(logFilePath, true);
                streamWriter.WriteLine(logString);
                streamWriter.Close();
                myProcessStartInfo =new ProcessStartInfo(yougetPath,arguments);
                myProcessStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                myProcessStartInfo.UseShellExecute = false;
                myProcessStartInfo.CreateNoWindow = false;
				myProcess.StartInfo=myProcessStartInfo;
				myProcess.Start();

				while(!myProcess.HasExited)
				{
					myProcess.WaitForExit();
				}
			}
			myProcess.Close();          
        }
	}
}