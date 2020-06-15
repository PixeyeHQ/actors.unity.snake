using System;
using Pixeye.Actors;
using UnityEngine;
using UnityEngine.UI;


namespace Pixeye.Source
{
  public class LayerUI : Layer<LayerUI>, IReceive<SignalGameUpdate>
  {
    public Text text;


    protected override void Setup()
    {
      AddSignal(this);
    }


    public void HandleSignal(in SignalGameUpdate arg)
    {
 
      text.text = $"LEVEL: {arg.level}\nSCORE:{arg.score}";
    }
  }
}