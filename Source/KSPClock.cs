//KSP v0.21+
using System;
using KSP.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[KSPAddon(KSPAddon.Startup.MainMenu, true)]
public class KSPClock : MonoBehaviour
{
    private static String VERSION = "1.2";  //Current version saved in config file for possible future use
    private const int WINDOWID = 7224;  //Unity window ID
    private static PluginConfiguration config = null;   //Config file

    private static int windowWidth = 75;    //Window width
    private static int windowHeight = 20;   //Window height
    private static Rect pos = new Rect(0, 0, windowWidth, windowHeight);  //Window position and size
    private static int mode = -1;  //Display mode, currently  0 for In-Flight, 1 for Editor, -1 to hide
    private static bool show24 = false; //Show 24 hour mode (false = 12 hour)

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Start()
    {
        //Create or load config file
        config = KSP.IO.PluginConfiguration.CreateForType<KSPClock>(null);
        config.load();

        //Load the last saved format
        show24 = config.GetValue<bool>("show24", false);
    }

    private void OnGUI()
    {
        //Get the current skin
        GUI.skin = HighLogic.Skin;

        if (FlightGlobals.fetch != null && FlightGlobals.ActiveVessel != null)  //Check if in flight
            checkMode(0);
        else if (EditorLogic.fetch != null) //Check if in editor
            checkMode(1);
        else   //Not in flight or in editor, unset the mode and hide the clock window
        {
            checkMode(-1);
            return;
        }

        //Make sure the window isn't dragged off screen
        if (pos.x < 0)
            pos.x = 0;
        if (pos.y < 0)
            pos.y = 0;
        if (pos.x > Screen.width - windowWidth)
            pos.x = Screen.width - windowWidth;
        if (pos.y > Screen.height - windowHeight)
            pos.y = Screen.height - windowHeight;

        //Draw the window with no title (doesn't display when the window is only 20 high)
        pos = GUI.Window(WINDOWID, pos, drawTime, "");
    }

    private void checkMode(int curMode)
    {
        //Check if the mode has changed
        if (mode != curMode)
        {
            //Save the position of the window in the previous mode
            saveConfig();

            //Change modes
            mode = curMode;

            //Load the saved position of the new mode
            if (mode >= 0)
            {
                config.load();
                pos = config.GetValue<Rect>("pos" + mode, new Rect(Screen.width - windowWidth - 230, mode == 0 ? 1 : 28, windowWidth, windowHeight));
            }
        }
    }
    
    private void saveConfig()
    {
        //Update the configuration file
        config.SetValue("version", VERSION);
        if(mode >= 0)
            config.SetValue("pos" + mode, pos);
        config.SetValue("show24", show24);
        config.save();
    }

    private void OnApplicationQuit()
    {
        //Save configuration file
        saveConfig();
    }

    private void drawTime(int windowID)
    {
        //Check for right click, which toggles between 12 hour and 24 hour mode
        if (Event.current.type == EventType.mouseUp && Event.current.button == 1)
        {
            if (show24)
                show24 = false;
            else
                show24 = true;
        }

        //Draw the local time centered within the window
        GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
        //centeredStyle.fontStyle = FontStyle.Bold;
        centeredStyle.alignment = TextAnchor.UpperCenter;

        //Format the time (default or 24 hour)
        String curTime = System.DateTime.Now.ToLocalTime().ToShortTimeString();
        if (show24)
        {
            //If the default is already 24 hour, toggle to 12 hour instead
            if (System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Equals("HH:mm") || System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Equals("H:mm"))
                curTime = System.DateTime.Now.ToLocalTime().ToString("h:mm tt");
            else
                curTime = System.DateTime.Now.ToLocalTime().ToString("HH:mm");
        }

        //Display the time
        GUI.Label(new Rect(5, 1, windowWidth - 10, windowHeight), curTime, centeredStyle);

        //Make window draggable
        GUI.DragWindow();
    }
}