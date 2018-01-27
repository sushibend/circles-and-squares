using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

using System.Net;
using System.Net.Sockets;

public class TCPClientHandler
{
    // data
    public const int numBytes = 57;
    public byte[] rcvBuf = new byte[numBytes];
    // status
    private const int maxIdleCnt = 10000;
    private int idleCnt;
    // thread
    private Thread thread;
    Socket clientSocket;
    NetworkStream networkStream;
    HauoliTracker hauoliTracker;
    int clientIndex;


    public TCPClientHandler(Socket s, HauoliTracker tracker, int index) {
        clientSocket = s;
        hauoliTracker = tracker;
        clientIndex = index;

        // Create TCP Client Stream
        Debug.Log("Accept Connection, Client Index = " + clientIndex);
        networkStream = new NetworkStream(clientSocket);
        idleCnt = 0;

        // Open thread to keep receiving
        thread = new Thread(new ThreadStart(TCPTrack));
        thread.Start();

    }

    public void TCPTrack() {
        while (idleCnt < maxIdleCnt) {
            int len = networkStream.Read(rcvBuf, 0, rcvBuf.Length);
            if(len > 0) {
                processRcvData(rcvBuf);
            }
            else {
                idleCnt ++;
            }
        }

        Debug.Log("Stop Client Index = " + clientIndex);
        hauoliTracker.ReleaseIndex(clientIndex);
    }


    public void processRcvData(byte[] buffer) {
        int idx = 0;

        // Timestamp   
        long timestamp = BitConverter.ToInt64(buffer, idx);
        idx += sizeof(long);

        // Coordinate
        for (int i = 0; i < 3; i++)
        {
            hauoliTracker.mCoor[clientIndex][i] = (float)BitConverter.ToDouble(buffer, idx); // / 1000
            // Debug.Log("coor = " + mCoor[i]);
            idx += sizeof(double);
        }

        // Gyroscope
        for (int i = 0; i < 3; i++)
        {
            hauoliTracker.mGyro[clientIndex][i] = (float)BitConverter.ToDouble(buffer, idx);
            // Debug.Log("gyro = " + mGyro[i]);
            idx += sizeof(double);
        }

        // Indicator
        bool reset = (buffer[idx] & 1) != 0;
        bool pressLeft = (buffer[idx] & (1 << 1)) != 0;
        bool pressRight = (buffer[idx] & (1 << 2)) != 0;
        // mouse = "reset=" + reset + ",left=" + pressLeft + ",right=" + pressRight;
        // Debug.Log("reset = " + reset);
        // Debug.Log("pressLeft = " + pressLeft);
        // Debug.Log("pressRight = " + pressRight);

        // Mouse press
        hauoliTracker.mouseClick[clientIndex] = pressRight;

        // Debug.Log ("hauoliTracker hand " + clientIndex + " rcv = " + hauoliTracker.mCoor[clientIndex][0] + "," + hauoliTracker.mCoor[clientIndex][1] + "," + hauoliTracker.mCoor[clientIndex][2]);
    }    
}
