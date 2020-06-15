using Unity.IL2CPP.CompilerServices;
using Pixeye.Actors;

namespace Pixeye.Source
{
  public class ComponentTile
  {
    public int tag;
    public int x;
    public int y;
  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  public static partial class Component
  {
    public const string Tile = "Pixeye.Source.ComponentTile";

    internal static ref ComponentTile componentTile(in this ent entity)
      => ref Storage<ComponentTile>.components[entity.id];
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  sealed class StorageComponentTile : Storage<ComponentTile>
  {
    public override ComponentTile Create() => new ComponentTile();

    public override void Dispose(indexes disposed)
    {
      foreach (var index in disposed)
      {
        ref var component = ref components[index];
      }
    }
  }

  #endregion
}