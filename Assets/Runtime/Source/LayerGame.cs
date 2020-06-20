using Pixeye.Actors;


namespace Pixeye.Source
{
  public class LayerGame : Layer<LayerGame>
  {
    protected override void Setup()
    {
      Add<ProcessorGame>();
      SceneSub.Add("Scene UI");
    }

    protected override void OnLayerDestroy()
    {
      SceneSub.Remove("Scene UI");
    }
  }
}