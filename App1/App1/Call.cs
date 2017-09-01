using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace App1
{
    class Call : ContentPage
    {
        
        class Message
        {
            public Message(string MessageString, TextAlignment Horizon_)
            {
                AMessage = MessageString;
                Horizon=Horizon_;
            }

            public string AMessage { private set; get; }
            public string Time { private set { } get { return DateTime.Now.ToLongTimeString().ToString(); } }
            public TextAlignment Horizon { private set; get; }
        };
        Button sendButton = new Button
        {
            Text = "Send",
            WidthRequest = 70,
            BackgroundColor = Color.Pink
        };

        Entry entryMessage = new Entry
        {
             Placeholder="Please write something here..",
             WidthRequest = 270
        };

        // Define some data.
        static ObservableCollection<Message> message = new ObservableCollection<Message>
        {

        };

        ListView MessageList = new ListView();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




        public Call()
        {
            BackgroundImage = "callbackground";
            message.Clear();
            Label header = new Label
            {
                WidthRequest=300,
                Text = "TwoTalk",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };
            Button outButton = new Button
            {
                Text = "Out",
                BackgroundColor = Color.Pink
            };
            StackLayout Headers = new StackLayout
            {
                Spacing = 5,
                HeightRequest = 40,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    header,
                    outButton

                }

            };

            outButton.Clicked += OverTalkButtun;


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            MessageList = new ListView
            {
                ItemsSource = message,
                SeparatorColor = Color.Transparent,

            ItemTemplate = new DataTemplate(() =>
                {
                    HeightRequest = 120;
                    Label timeLabel = new Label();
                    Label messageLabel = new Label();
                    // Create views with bindings for displaying each property.
                    timeLabel.SetBinding(Label.TextProperty, "Time");
                    timeLabel.HeightRequest = 20;
                    timeLabel.SetBinding(Label.HorizontalTextAlignmentProperty, "Horizon");
                    timeLabel.VerticalTextAlignment = TextAlignment.Start;
                    messageLabel.SetBinding(Label.TextProperty, "AMessage");
                    messageLabel.HeightRequest = 20;
                    messageLabel.VerticalTextAlignment = TextAlignment.End;
                    messageLabel.SetBinding(Label.HorizontalTextAlignmentProperty, "Horizon");
                    messageLabel.TextColor = Color.Black;
                    messageLabel.FontSize = 20;


                    return new ViewCell
                    {
                        Height =120,
                        View = new StackLayout
                        {
                            BackgroundColor = Color.Transparent,
                            HeightRequest=120,
                            Padding = new Thickness(5, 5,5,5),
                            Children =
                            {
                                timeLabel,
                                messageLabel
                            }
                        }
                    };
                })

            };
            
            sendButton.Clicked += SendMessageTOListButtun;
            
            
            StackLayout Send = new StackLayout
            {
                Spacing = 5,
                HeightRequest=60,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    entryMessage,
                    sendButton
                }

            };


            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // Build the page.
            this.Content = new StackLayout
            {
                Children =
                {
                    Headers,
                    MessageList,
                    Send
                }
            };
            var p = new IfTalk();
            p.SetIf(true);
            Task.Run(() =>
            {
                while (true)
                {
                    if (IfTalk.If == false)
                        break;
                    var data_ = new Byte[270];
                    TCPListener.stream.Read(data_, 0, data_.Length);
                    MessageStruct m = BytesToStruct<MessageStruct>(data_);
                    if (!m.ifTo)
                    {
                        if (!m.ifOver)
                            Device.BeginInvokeOnMainThread(() => SendMessageTOList(BytesToStruct<MessageStruct>(data_).str,TextAlignment.Start));
                        else
                        {
                            IfTalk.If = false;
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Android.Widget.Toast.MakeText(Forms.Context, "your listener has left..", Android.Widget.ToastLength.Short).Show();
                                var page = new Match();
                                Application.Current.MainPage = new Match();
                            });


                        }
                    }
                }
            });
        }
        void OverTalkButtun(object sender, EventArgs e)
        {
            IfTalk.If = false;
            MessageStruct messageStruct = new MessageStruct("", true,true);
            TCPListener.stream.Write(StructToBytes(messageStruct), 0, StructToBytes(messageStruct).Length);
            var page = new Match();
            Application.Current.MainPage = new Match();
        }
        void SendMessageTOListButtun(object sender, EventArgs e)
        {
            MessageStruct messageStruct=new MessageStruct(entryMessage.Text,true,false);
            TCPListener.stream.Write(StructToBytes(messageStruct), 0, StructToBytes(messageStruct).Length);
            SendMessageTOList(entryMessage.Text, TextAlignment.End);
            entryMessage.Text = "";
        }
        void SendMessageTOList(string messageString, TextAlignment Horizon_)//BindableProperty horizon
        {
            message.Add(new Message(messageString, Horizon_));
            MessageList.ScrollTo(message[message.Count - 1], ScrollToPosition.End, true);

        }
        public static byte[] StructToBytes(object structObj)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(structObj);
            //创建byte数组
            byte[] bytes = new byte[size];
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);
            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回byte数组
            return bytes;
        }
        public static T BytesToStruct<T>(byte[] bytes)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(typeof(T));
            //byte数组长度小于结构体的大小
            if (size > bytes.Length)
            {

            }
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            //将内存空间转换为目标结构体
            T obj = Marshal.PtrToStructure<T>(structPtr);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回结构体
            return obj;
        }


    }
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct MessageStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string str;
        public bool ifTo;
        public bool ifOver;
        public MessageStruct(string str, bool ifTo, bool ifOver)
        {

            this.str = str;
            //是否要传到另一个客户端的流中
            this.ifTo = ifTo;
            //是否是关闭talk的message
            this.ifOver = ifOver;
        }
    }

}