using Pixeye.Actors;


namespace Pixeye.Source
{
  public class LayerGame : Layer<LayerGame>
  {
    protected override void Setup()
    {
#if UNITY_2019_4_OR_NEWER
#else
#endif
    
    }

    protected override void OnLayerDestroy()
    {
      SceneSub.Remove("Scene UI");
    }
  }
}