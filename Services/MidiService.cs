using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MSIMAU.Services;
 

using static Microsoft.Maui.Storage.Preferences;

using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

public class MidiService
{
    public readonly string _defaultValue = "USB-MIDI";
    public ICollection<InputDevice>? InputDevices;
    public ICollection<OutputDevice>? OutputDevices;
    public InputDevice? InputDevice;
    public OutputDevice? OutputDevice;

    public MidiService()
    {
        Preferences.Default.Clear();
        InitializeMidiInputOutputDevices();
        if (InputDevice != null)
        {
            InputDevice.EventReceived += (obj, e) =>
            {
                var midiEvent = e.Event;
                if (midiEvent is NoteOnEvent noteOnEvent)
                {
                    Console.WriteLine($"Note On: {noteOnEvent.NoteNumber}, Velocity: {noteOnEvent.Velocity}");
                    SendNoteOut(noteOnEvent.NoteNumber, noteOnEvent.Velocity, 1000);
                }
                else if (midiEvent is NoteOffEvent noteOffEvent)
                {
                    Console.WriteLine($"Note Off: {noteOffEvent.NoteNumber}");
                }
            };
        }

        if (OutputDevice != null)
        {
            OutputDevice.EventSent += (obj, e) => { Console.WriteLine("MIDI Output event sent."); };
        }
    }

    private void InitializeMidiInputOutputDevices()
    {
        InputDevices = Melanchall.DryWetMidi.Multimedia.InputDevice.GetAll();
        foreach (InputDevice inputDevice in InputDevices)
        {
            Console.WriteLine($"MIDI Input Device: {inputDevice.Name}");
        }

        OutputDevices = Melanchall.DryWetMidi.Multimedia.OutputDevice.GetAll();
        foreach (OutputDevice device in OutputDevices)
        {
            Console.WriteLine($"MIDI Output Device: {device.Name}");
        }

        InitializeMidiDevices();


        _ = InputDevice.Connect();
        InputDevice?.StartEventsListening();
    }


    private void InitializeMidiDevices()
    {

        try
        {
            InputDevice = GetAndSetDevice(InputDevices, "MidiInput", _defaultValue);
            OutputDevice = GetAndSetDevice(OutputDevices, "MidiOutput", _defaultValue);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private T? GetAndSetDevice<T>(ICollection<T>? devices, string preferenceKey, string defaultName) where T : class
    {
        var savedDeviceName = Default.Get(preferenceKey, defaultValue: defaultName);

        var selectedDevice = devices?.FirstOrDefault(x => GetDeviceName(x) == savedDeviceName) ??
                             devices?.FirstOrDefault();
        Default.Set(preferenceKey, GetDeviceName(selectedDevice));

        return selectedDevice;
    }

    private string GetDeviceName<T>(T? device) where T : class
    {
        var deviceAsDynamic = device as dynamic;
        return deviceAsDynamic?.Name ?? string.Empty;
    }


    public void SendNoteOut(int noteNumber, int velocity, int duration)
    {
        if (OutputDevice == null)
        {
            throw new InvalidOperationException("Output device is not initialized.");
            //return;
        }

        var noteOnEvent = new NoteOnEvent((SevenBitNumber)noteNumber, (SevenBitNumber)velocity)
        {
            Channel = (FourBitNumber)1,
            Velocity = (SevenBitNumber)120,
        };
        OutputDevice.SendEvent(noteOnEvent);
        Task.Delay(duration).ContinueWith(_ =>
        {
            var noteOffEvent = new NoteOffEvent((SevenBitNumber)noteNumber, (SevenBitNumber)0);
            OutputDevice.SendEvent(noteOffEvent);
        });
    }
}
