using System;
using Unity.IL2CPP.CompilerServices;
using Pixeye.Actors;
using UnityEngine;


namespace Pixeye.Source
{
  public struct Body
  {
    public int x;
    public int y;
  }

  [Serializable]
  public class ComponentSnake
  {
    public Body[] body;
    public int length = 1;

    public int directionX = 1;
    public int directionY = 0;
  }

  #region HELPERS

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  public static partial class Component
  {
    public const string Snake = "Pixeye.Source.ComponentSnake";

    internal static ref ComponentSnake componentSnake(in this ent entity)
      => ref Storage<ComponentSnake>.components[entity.id];
  }

  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
  [Il2CppSetOption(Option.DivideByZeroChecks, false)]
  sealed class StorageComponentSnake : Storage<ComponentSnake>
  {
    public override ComponentSnake Create() => new ComponentSnake();

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