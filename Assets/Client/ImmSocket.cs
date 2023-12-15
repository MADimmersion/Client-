
using System;

using System.Collections;

using System.Collections.Generic;

using System.Linq;

using System.Net.Sockets;

using System.Net;

using System.Text;

using System.Threading;

using UnityEngine;





public class ImmSocket

{

    #region Properties

    string _ip;

    int _port;



    TcpClient _tcpClient;

    NetworkStream _stream = null;



    Thread _socketThread;

    Thread _sendingThread;



    List<Byte[]> _packetsToSend;

    Mutex _packetsToSendMutex;



    bool _isConnected;



    protected bool m_Running_receive = false;

    protected bool m_Running_send = false;



    public System.Object Tag { get; set; }

    #endregion



    #region events



    public delegate void delegatePacketReceive(ImmSocket sender, byte[] packet);

    public event delegatePacketReceive packetReceive;



    public delegate void delegateConnected(ImmSocket sender);

    public event delegateConnected connected;



    public delegate void delegateDisconnected(ImmSocket sender);

    public event delegateDisconnected disconnected;



    IAsyncResult connexion_result;

    #endregion

    public ImmSocket()

    {



        init();

    }

    public ImmSocket(TcpClient tcpClient)

    {

        if (tcpClient.Connected)

        {

            _tcpClient = tcpClient;

            init();



            if (connected != null)

            {

                connected(this);

            }

            _isConnected = true;



            _socketThread = new Thread(() => socketThread());

            _socketThread.IsBackground = true;

            _socketThread.Start();



            _sendingThread = new Thread(() => sendingThread());

            _sendingThread.IsBackground = true;

            _sendingThread.Start();

        }





    }



    private void init()

    {

        _isConnected = false;

        _packetsToSend = new List<byte[]>();

        _packetsToSendMutex = new Mutex();



    }





    public bool connect(string ip, int port)

    {

        if (_isConnected)

        {

            return false;

        }



        try

        {

            _ip = ip;

            _port = port;



            _tcpClient = new TcpClient();

            _tcpClient.NoDelay = true;

            connexion_result = _tcpClient.BeginConnect(_ip, _port, OnConnect, null);



            return true;

        }

        catch

        {

            return false;

        }

    }



    public void cancel_connection()

    {

        _tcpClient.Close();



    }



    private void OnConnect(IAsyncResult ar)

    {

        if (_tcpClient.Connected)

        {

            _isConnected = true;



            if (connected != null)

            {

                connected(this);

            }



            _socketThread = new Thread(() => socketThread());

            _socketThread.IsBackground = true;

            _socketThread.Start();



            _sendingThread = new Thread(() => sendingThread());

            _sendingThread.IsBackground = true;

            _sendingThread.Start();

        }

        else

        {

            StopAll();

        }



    }



    private void socketThread()

    {

        try

        {

            DateTime lLastReceiveMessageTime = DateTime.Now;

            m_Running_receive = true;



            _stream = _tcpClient.GetStream();





            while (m_Running_receive && _isConnected)

            {

                int size = 0;

                var intBuffer = new byte[4];



                if (_stream.DataAvailable)

                {

                    size = _stream.Read(intBuffer, 0, 4);

                    lLastReceiveMessageTime = DateTime.Now;

                    if (size > 0)

                    {

                        int sizeToRead = BitConverter.ToInt32(intBuffer, 0);



                        if (sizeToRead > 0)

                        {

                            var packet = new byte[sizeToRead];

                            size = 0;

                            do

                            {

                                size += _stream.Read(packet, size, sizeToRead - size);

                            }

                            while (size < sizeToRead);



                            if (packetReceive != null)

                            {

                                packetReceive(this, packet);

                            }

                        }







                    }

                }



                else

                {

                    if ((DateTime.Now - lLastReceiveMessageTime).TotalMilliseconds > 20000)

                    {

                        _isConnected = false;

                    }

                    Thread.Sleep(1);

                }



            }

        }

        catch (SocketException ex)

        {

            string d = ex.Message;

            Debug.Log(ex);

        }

        catch (Exception ex)

        {

            string d = ex.Message;

            Debug.Log(ex);

        }

        StopAll();

    }



    private void sendingThread()

    {

        DateTime lLastSendMessageTime = DateTime.Now;






        try

        {





            m_Running_send = true;

            while (m_Running_send && _isConnected)

            {





                if (_stream != null)

                {

                    if (_stream.CanWrite)

                    {

                        _packetsToSendMutex.WaitOne();

                        while (_packetsToSend.Count != 0)

                        {

                            var packetToSend = _packetsToSend.First();

                            _packetsToSend.RemoveAt(0);



                            _stream.Write(BitConverter.GetBytes(packetToSend.Length), 0, 4);

                            _stream.Write(packetToSend, 0, packetToSend.Length);

                            lLastSendMessageTime = DateTime.Now;

                        }

                        _packetsToSendMutex.ReleaseMutex();





                        //Debug.Log(time);



                        if ((DateTime.Now - lLastSendMessageTime).TotalMilliseconds > 2000)

                        {

                            //Debug.Log("HEARTBEAT");

                            _stream.Write(BitConverter.GetBytes(0), 0, 4);

                            lLastSendMessageTime = DateTime.Now;

                        }

                    }

                }





                Thread.Sleep(1);



            }

        }

        catch (SocketException ex)

        {

            string d = ex.Message;

        }

        catch (Exception ex)

        {

            string d = ex.Message;

        }

    }



    public void sendPacket(byte[] packet)

    {

        _packetsToSendMutex.WaitOne();

        _packetsToSend.Add(packet);

        _packetsToSendMutex.ReleaseMutex();

    }



    public string getStringFromPaquet(byte[] packet)

    {

        UTF8Encoding utf8enc = new UTF8Encoding();

        return utf8enc.GetString(packet);

    }



    public byte[] getPaquetFromString(String data)

    {

        UTF8Encoding utf8enc = new UTF8Encoding();

        return utf8enc.GetBytes(data);

    }



    public void close()

    {

        m_Running_send = false;

        m_Running_receive = false;

        /*if (_socketThread != null)

        {

            _socketThread.Join();

        }

        if (_sendingThread != null)

        {

            _sendingThread.Join();

        }*/



    }



    private void StopAll()

    {

        Debug.Log("StopAll");

        _isConnected = false;



        if (_tcpClient == null) return;



        if (_stream != null) _stream.Close();

        _stream = null;



        if (_tcpClient != null) _tcpClient.Close();

        _tcpClient = null;



        if (disconnected != null)

        {

            disconnected(this);

        }

    }



    public string getIpAddress()

    {

        return ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString();

    }

}