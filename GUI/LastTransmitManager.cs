using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.GUI;

/// <summary>
/// Keep track of which is the last aircraft transmitting.
/// </summary>
public class LastTransmitManager
{
    private readonly List<VSCSFrequency> _monitoredFrequencies = [];

    /// <summary>
    /// Gets or sets the callsign of the aircraft that last transmitted.
    /// </summary>
    public string LastReceivedFrom { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="LastTransmitManager"/> class.
    /// </summary>
    public LastTransmitManager()
    {
        Audio.VSCSFrequenciesChanged += RemonitorFrequencies;
    }

    private void RemonitorFrequencies(object src, EventArgs e)
    {
        foreach (var freq in _monitoredFrequencies)
        {
            freq.ReceivingChanged -= FreqReceived;
        }

        _monitoredFrequencies.Clear();

        foreach (var freq in Audio.VSCSFrequencies)
        {
            _monitoredFrequencies.Add(freq);

            freq.ReceivingChanged += FreqReceived;
        }
    }

    private void FreqReceived(object src, EventArgs e)
    {
        var callsign = (src as VSCSFrequency)?.ReceivingCallsigns.FirstOrDefault() ?? LastReceivedFrom;

        // Don't match ATC.
        // e.g: SY_GND, ML-BLA_CTR
        if (!callsign.Contains('_'))
        {
            LastReceivedFrom = callsign;
        }
    }
}
