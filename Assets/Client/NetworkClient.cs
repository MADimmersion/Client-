using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class NetworkClient : MonoBehaviour
{
    ImmSocket tcpSocket;

    bool isConnected;

    IPEndPoint endpoint;



    /// <summary>
    /// Cloud Info
    /// </summary>
    public GameObject pointCloud;
    public PointCloud PCScript;

    private Vector3 center = Vector3.zero;
    private float radius = 1f;

    /// <summary>
    /// IP change Info
    /// </summary>
    /// 

    public string ip = "127.000.000.000";

    bool isNewActiveConnexion = false;
    bool isNewDisconnexion = false;
    bool isNewLoading = false;



    bool isIPUpdateRequested = false;
    bool isLoading = false;

    public UnityEvent Connected = new UnityEvent();
    public UnityEvent Loading = new UnityEvent();
    public UnityEvent Disconnected = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {

        //      #region TCProtocol
        //      tcpSocket = new ImmSocket();

        //      tcpSocket.connected += TcpSocket_connected;

        //      tcpSocket.disconnected += TcpSocket_disconnected;

        //      tcpSocket.packetReceive += TcpSocket_packetReceive;

        //      isConnected = false;

        //tcpSocket.connect("127.0.0.1", 27003);
        //      #endregion

        #region PointCloud Info


        pointCloud = GameObject.Find("PointCloud");
        // Get a reference to the ScriptA component attached to the same GameObject
        PCScript = pointCloud.GetComponent<PointCloud>();

        center = PCScript.center;
        radius = PCScript.radius;

        #endregion

    }

    private void TcpSocket_disconnected(ImmSocket sender)
    {
        Debug.Log("Disconnected");
        isConnected = false;
        isLoading = false;
        isNewDisconnexion = true;
    }

    private void TcpSocket_connected(ImmSocket sender)
    {
        Debug.Log("Connected");
        isConnected = true;
        isLoading = false;
        isNewActiveConnexion = true;
    }

    private void TcpSocket_packetReceive(ImmSocket sender, byte[] packet) //receive data
    {
        int packetLength = packet.Length;
        //Initializes the center and radius (size 20)
        if (packetLength == 20)
        {
            center.x = System.BitConverter.ToSingle(packet, 0);
            center.y = System.BitConverter.ToSingle(packet, 4);
            center.z = System.BitConverter.ToSingle(packet, 8);
            PCScript.center = center;

            radius = System.BitConverter.ToSingle(packet, 12);
            PCScript.radius = radius;

            Debug.Log("center" + center + " radius" + radius);

            PCScript.maxNbOfVertices = System.BitConverter.ToInt32(packet, 16);
        }

        //Change center position (size 12)
        if (packetLength == 12)
        {
            center.x = System.BitConverter.ToSingle(packet, 0);
            center.y = System.BitConverter.ToSingle(packet, 4);
            center.z = System.BitConverter.ToSingle(packet, 8);
            PCScript.center = center;
            Debug.Log("center" + center);
        }

        //Change radius (size 4)
        if (packetLength == 4)
        {
            radius = System.BitConverter.ToSingle(packet);
            PCScript.radius = radius;
            Debug.Log("in");
        }

        //Receive Point Cloud if enough points are sent (size over 200 points and 200 colors => 200*3*4 + 200*4*4 = 5600)
        //First 4 bytes is the number of vertices
        if (packetLength > 5600 && !PCScript.isUpdated)
        {
            int verticesLength = System.BitConverter.ToInt32(packet, 0);
            //Array of coordinates for each point in PointCloud
            Vector3[] vertices = new Vector3[verticesLength];
            //Array of colors corresponding to each point in PointCloud
            Color32[] colors = new Color32[verticesLength];
            int nextByte = 4;
            for (int i = 0; i < verticesLength; i++)
            {
                vertices[i].x = System.BitConverter.ToSingle(packet, nextByte);

                vertices[i].y = System.BitConverter.ToSingle(packet, nextByte + 4);
                vertices[i].z = System.BitConverter.ToSingle(packet, nextByte + 8);

                nextByte += 12;

                colors[i].r = packet[nextByte];
                colors[i].g = packet[nextByte + 1];
                colors[i].b = packet[nextByte + 2];
                colors[i].a = packet[nextByte + 3];
                nextByte += 4;
            }
            PCScript.Vertices = vertices;
            PCScript.Colors = colors;
            PCScript.isUpdated = true;
        }
        DecodeMesh(packet);
    }

    //Connect to server
    public void connect()
    {
        tcpSocket = new ImmSocket();

        tcpSocket.connected += TcpSocket_connected;

        tcpSocket.disconnected += TcpSocket_disconnected;

        tcpSocket.packetReceive += TcpSocket_packetReceive;

        tcpSocket.connect(ip, 27003);
        isLoading = true;
        isNewLoading = true;
    }

    public void disconnect()
    {
        if (tcpSocket != null)
        {
            tcpSocket.close();
            isLoading = true;
            isNewLoading = true;
        }
    }

    public void Cancel_connection()
    {
        tcpSocket.cancel_connection();
        isLoading = false;

        Disconnected.Invoke();
        if (tcpSocket != null)
        {
            tcpSocket.connected -= TcpSocket_connected;
            tcpSocket.disconnected -= TcpSocket_disconnected;
            tcpSocket.packetReceive -= TcpSocket_packetReceive;
            tcpSocket = null;
        }
    }
    public void UpdateServerIP(string IP)
    {
        ip = IP;
        isIPUpdateRequested = true;

        if (isConnected)
        {
            disconnect();
        }

    }

    public void sendPacket(byte[] pFrame)
    {

        if (isConnected)
        {
            tcpSocket.sendPacket(pFrame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoading)
        {
            if (isNewLoading)
            {
                Loading.Invoke();
                isNewLoading = false;
            }
        }


        else
        {
            if (isConnected && isNewActiveConnexion)
            {
                Connected.Invoke();
                isNewActiveConnexion = false;
            }
            if (!isConnected && isNewDisconnexion)
            {
                Disconnected.Invoke();
                isNewDisconnexion = false;



                if (tcpSocket != null)
                {
                    tcpSocket.connected -= TcpSocket_connected;



                    tcpSocket.disconnected -= TcpSocket_disconnected;



                    tcpSocket.packetReceive -= TcpSocket_packetReceive;



                    tcpSocket = null;
                }
            }
            if (isIPUpdateRequested)
            {
                Debug.Log("REQUESTED");
                connect();
                isIPUpdateRequested = false;
            }
        }
    }

    private void DecodeMesh(byte[] packet)
    {
        if (packet[0] == 42)
        {
            int meshDataLength = packet.Length - 1;
            // Create a new byte array without the added byte
            byte[] meshBytes = new byte[meshDataLength];
            Array.Copy(packet, 1, meshBytes, 0, meshDataLength);
            // Create a MemoryStream from received data
            using (MemoryStream memoryStream = new MemoryStream(meshBytes, 0, meshDataLength))
            {
                // Deserialize the mesh using BinaryFormatter
                Mesh receivedMesh = DeserializeMesh(memoryStream);

                // Use the received mesh in your scene
                PCScript.mesh = receivedMesh;
            }

        }
    }
    private Mesh DeserializeMesh(Stream stream)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        return (Mesh)formatter.Deserialize(stream);
    }

    public void OnApplicationQuit()
    {
        if (isConnected)
        {
            tcpSocket.close();
        }

    }

}
