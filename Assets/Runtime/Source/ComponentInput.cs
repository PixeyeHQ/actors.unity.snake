using System;
using Unity.IL2CPP.CompilerServices;
using Pixeye.Actors;
using UnityEngine;

namespace Pixeye.Source
{
  [Serializable]
  public class ComponentInput
  {
    public KeyCode Left;
    public KeyCode Up;
    public KeyCode Right;
    public KeyCode Down;

  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  public static partial class Component
  {
    public const string Input = "Pixeye.Source.ComponentInput";

    internal static ref ComponentInput componentInput(in this ent entity)
      => ref Storage<ComponentInput>.components[entity.id];
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  sealed class StorageComponentInput : Storage<ComponentInput>
  {
    public override ComponentInput Create() => new ComponentInput();

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