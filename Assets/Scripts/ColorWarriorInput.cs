using System;
using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;


public static class ColorWarriorInput
{
    public static SerialPort Serial;
    private static Dictionary<Color, byte> Colors;
    private static Dictionary<int, byte> ColorID; 
    private static Dictionary<int, int> TouchesID;
    private static Dictionary<int, byte> ToucheColorID;
    private static bool[] Touches;
    private static bool[] TouchesStateMachine;

    public static void Init()
    {
        Connect();
        Touches = new bool[4];
        TouchesStateMachine = new bool[4];
        Colors = new Dictionary<Color, byte>();
        ColorID = new Dictionary<int, byte>();
        TouchesID = new Dictionary<int, int>();
        ToucheColorID = new Dictionary<int, byte>();

        Colors.Add(Color.red, 0);
        Colors.Add(Color.green, 2);
        Colors.Add(Color.blue, 4);

        ColorID.Add(0, 6);
        ColorID.Add(1, 0);
        ColorID.Add(2, 4);
        ColorID.Add(3, 2);

        TouchesID.Add(0, 0);
        TouchesID.Add(6, 1);
        TouchesID.Add(4, 2);
        TouchesID.Add(2, 3);

        ToucheColorID.Add(0, 0);
        ToucheColorID.Add(1, 48);
        ToucheColorID.Add(2, 32);
        ToucheColorID.Add(3, 16);

        Serial.ReadTimeout = 5;
    }

    private static byte GetColorId(Color c)
    {
        if (Colors.ContainsKey(c))
            return (Colors[c]);
        return (6);
    }

    public static bool IsConnected()
    {
        if (Serial != null && Serial.IsOpen)
            return (true);
        Serial = null;
        return (false);
    }

    private static void RefreshInputs()
    {
        byte[] c = new byte[1];
        int value = 0;

        try
        {
            while (true)
            {
                bool pressed = false;

                Serial.Read(c, 0, 1);
                value = Convert.ToInt32(c[0]);
                if (value % 2 != 0)
                {
                    --value;
                    pressed = true;
                }
                Touches[TouchesID[value]] = pressed;
            }
        }
        catch (TimeoutException)
        {
        }
    }

    public static void UpdateState()
    {
        int count = 0;

        foreach (var touch in Touches)
        {
            TouchesStateMachine[count] = touch;
            ++count;
        }
    }

    public static bool GetButtonDown(int t)
    {
        RefreshInputs();
        UpdateState();

        bool a = Touches[t] && Touches[t] != TouchesStateMachine[t];

        return (a);
    }

    public static bool GetInput(int t)
    {
        RefreshInputs();
        
        return (Touches[t]);
    }

    public static bool[] GetInputs()
    {
        RefreshInputs();
        return (Touches);
    }

    public static void SetButtonColor(int id, Color color)
    {
        byte b = GetColorId(color);

        b += ToucheColorID[id];
        Communicate(b);
    }

    public static void SetButtonColor(int id, int color)
    {
        byte b = ColorID[color];

        b += ToucheColorID[id];
        Communicate(b);
    }

    public static void Connect()
    {
        foreach (var portName in SerialPort.GetPortNames())
        {
            Debug.Log(portName);
            Serial = new SerialPort(portName, 9600);
            Serial.Open();
            return;
        }
    }

    public static void Communicate(byte b)
    {
        byte[] B = new[] {b};

        Serial.Write(B, 0, 1);
    }

    public static void Close()
    {
        Serial.Close();
    }
}
