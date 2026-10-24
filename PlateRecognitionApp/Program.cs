using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlateRecognitionApp
{
    /// <summary>应用程序入口；负责启动 WinForms 消息循环，不承载车牌识别业务逻辑。</summary>
    internal static class Program
    {
        // STA 是 WinForms UI 线程所用的单线程 apartment 模式，供窗体和系统对话框等组件交互。
        /// <summary>配置 WinForms 外观并创建主窗体；窗体关闭后应用程序消息循环结束。</summary>
        [STAThread]
        static void Main()
        {
            // 使用 Windows 当前视觉样式绘制标准 WinForms 控件。
            Application.EnableVisualStyles();
            // 让标准 WinForms 控件使用 GDI 文字绘制，而不是 UseCompatibleTextRendering 的旧式 GDI+ 绘制。
            Application.SetCompatibleTextRenderingDefault(false);
            // 创建 UI 层窗体并启动消息循环；按钮事件随后由窗体转调 BLL 服务。
            Application.Run(new Form1());
        }
    }
}
