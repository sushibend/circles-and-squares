using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

using System.Net;
using System.Net.Sockets;

public class HauoliTracker
{
    // TCP
    private int tcpPort = 10000;

    // Threading
    public const int maxConn = 2;
    public volatile bool running;
    private Thread thread;
    private int numConn = 0;
    List<int> availableIndex = new List<int>();

    // Buffer data
    public const int maxDim = 3;
    // private DateTime[] dt = new DateTime[maxConn];
    public float[][] mCoor = new float[maxConn][];
    public float[][] mGyro = new float[maxConn][];
    public bool[] mouseClick = new bool[maxConn];
    // public string mouse;


    public HauoliTracker()
    {
        // Initialize variables
        for(int i = 0; i < maxConn; i ++) {
            mCoor[i] = new float[maxDim];
            mGyro[i] = new float[maxDim];
            availableIndex.Add(i);
        }

        // Start a thread to wait for connection
        thread = new Thread(new ThreadStart(TCPTrack));
        thread.Start();
    }

    ~HauoliTracker() 
    {
        running = false;
    }

    void TCPTrack()
    {
        Debug.Log("Start a TCP server.");
        TcpListener tcpListener = new TcpListener(IPAddress.Any, tcpPort);
        tcpListener.Start();
        running = true;

        while(running) {
            while(availableIndex.Count == 0) {
                Thread.Sleep(100);
                continue;
            }
            
            numConn ++;
            int index = availableIndex[0];
            availableIndex.Remove(index);
            Debug.Log("Wait for connection #" + numConn + ", index = " + index);

            Socket soc = tcpListener.AcceptSocket();
#pragma warning disable RECS0026 // Possible unassigned object created by 'new'
            new TCPClientHandler(soc, this, index);
#pragma warning restore RECS0026 // Possible unassigned object created by 'new'
        }

        tcpListener.Stop();
    }

    public int GetNumberOfConnection(){
        return numConn - 1;
    }

    public void ReleaseIndex(int index) 
    {
        availableIndex.Add(index);
    }
    
    public void Interrupt()
    {
        // Debug.Log("Interrupt sent to thread, closing serialPort.");
        // running = false;
    }
}
