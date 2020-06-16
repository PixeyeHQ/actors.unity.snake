using Pixeye.Actors;
using UnityEngine;
using Random = Pixeye.Actors.Random;

namespace Pixeye.Source
{
  public class ProcessorGame : Processor, ITick
  {
    const int WIDTH = 26;
    const int HEIGHT = 14;

    public int level;
    public int score;
    public float blinkTime;

    readonly float[] blinkTimer = {0.5f, 0.6f, 0.7f, 0.8f, .9f, 1f, 1.1f, 1.2f};
    readonly float[] steps = {0.3f, 0.2f, 0.18f, 0.14f, .12f, 0.1f, 0.075f, 0.06f};
    readonly Color snakeColorBlink = new Color(215 / 255f, 255f / 255f, 225 / 255f);
    readonly Color snakeColor = new Color(122 / 255f, 195 / 255f, 135 / 255f);
    readonly Color foodColor = new Color(212 / 255f, 49 / 255f, 73 / 255f);
    readonly Color obstacleColor = new Color(0.5f, 0.5f, 0.5f, 1);
    readonly Color freeColor = new Color(1, 1, 1, 0);

    Group<ComponentInput, ComponentSnake> snakes;
    Group<ComponentTile> tiles;

    ent[,] tileMap;
    float step;

    public ProcessorGame()
    {
      step = steps[level];

      tileMap = new ent[WIDTH, HEIGHT];
      for (int xx = 0; xx < WIDTH; xx++)
      for (int yy = 0; yy < HEIGHT; yy++)
      {
        var tile  = Entity.Create("Obj Tile", new Vector3(xx, yy));
        var ctile = tile.Set<ComponentTile>();
        ctile.x         = xx;
        ctile.y         = yy;
        tileMap[xx, yy] = tile;

        if (xx == 0 || xx == WIDTH - 1)
          ctile.tag = Tags.Obstacle;
        else if (yy == 0 || yy == HEIGHT - 1)
          ctile.tag    = Tags.Obstacle;
        else ctile.tag = Tags.Empty;
      }

      // create snake 
      CreateSnake(13, 7);

      // create first food
      CreateFood(10, 5);

      Render();
    }

    public void CreateSnake(int x, int y)
    {
      var snake  = Entity.Create();
      var csnake = snake.Set<ComponentSnake>();
      var cinput = snake.Set<ComponentInput>();


      csnake.length                    = 1;
      csnake.body                      = new Body[WIDTH * HEIGHT];
      csnake.body[csnake.length - 1].x = x;
      csnake.body[csnake.length - 1].y = y;


      cinput.Up    = KeyCode.W;
      cinput.Left  = KeyCode.A;
      cinput.Down  = KeyCode.S;
      cinput.Right = KeyCode.D;

      var nextTile = GetTile(x, y);
      nextTile.componentTile().tag = Tags.Snake;
    }

    public void CreateEmpty(int x, int y)
    {
      var nextTile = GetTile(x, y);
      nextTile.componentTile().tag = Tags.Empty;
    }

    public void CreateFood(int x, int y)
    {
      var nextTile = GetTile(x, y);
      nextTile.componentTile().tag = Tags.Food;
    }

    public void CreateFoodRandomTile()
    {
      var nextTile = default(ent);
      do nextTile = GetTile(Random.Range(1, 25), Random.Range(1, 14));
      while (nextTile.componentTile().tag != Tags.Empty);
      nextTile.componentTile().tag = Tags.Food;
    }

    public ent GetTile(int x, int y)
    {
      return tileMap[x, y];
    }

    public ent GetTile(in Body snakeBody)
    {
      return tileMap[snakeBody.x, snakeBody.y];
    }

    public void Tick(float dt)
    {
      // input
      foreach (var snake in snakes)
      {
        var cinput = snake.componentInput();
        var csnake = snake.componentSnake();


        if (Input.GetKeyDown(cinput.Left) && csnake.directionX != 1)
        {
          csnake.directionY = 0;
          csnake.directionX = -1;
        }
        else if (Input.GetKeyDown(cinput.Right) && csnake.directionX != -1)
        {
          csnake.directionY = 0;
          csnake.directionX = 1;
        }
        else if (Input.GetKeyDown(cinput.Down) && csnake.directionY != 1)
        {
          csnake.directionX = 0;
          csnake.directionY = -1;
        }
        else if (Input.GetKeyDown(cinput.Up) && csnake.directionY != -1)
        {
          csnake.directionX = 0;
          csnake.directionY = 1;
        }
 
      }

      // logic
      if ((step -= dt) <= 0)
      {
        step += steps[level];
        foreach (var snake in snakes)
        {
          var     csnake      = snake.componentSnake();
          ref var snakeLength = ref csnake.length;

          var head = csnake.body[0];
          head.x += csnake.directionX;
          head.y += csnake.directionY;

          var nextTile = GetTile(head);
          var ctile    = nextTile.componentTile();

          switch (ctile.tag)
          {
            case Tags.Food:
            {
              CreateFoodRandomTile();
              ref var newBodySegment = ref csnake.body[snakeLength++];
              newBodySegment.x = head.x;
              newBodySegment.y = head.y;

              score += 50;

              switch (snakeLength)
              {
                case 3:
                  level = 1;

                  break;
                case 6:
                  level = 2;
                  break;
                case 9:
                  level = 3;
                  break;
                case 13:
                  level = 5;
                  break;
                case 18:
                  level = 6;
                  break;
                case 24:
                  level = 7;
                  break;
              }

              blinkTime = blinkTimer[level];
              SignalGameUpdate s;
              s.level = level;
              s.score = score;
              LayerUI.Send(s);
            }
              break;
            case Tags.Obstacle:
            {
              SceneMain.ChangeTo(0);
            }
              break;
            case Tags.Snake:
            {
              SceneMain.ChangeTo(0);
            }
              break;
          }

          var tail = csnake.body[snakeLength - 1];
          GetTile(tail).componentTile().tag = Tags.Empty;

          for (int i = snakeLength - 1; i > 0; i--)
          {
            csnake.body[i]                              = csnake.body[i - 1];
            GetTile(csnake.body[i]).componentTile().tag = Tags.Snake;
          }

          csnake.body[0].x += csnake.directionX;
          csnake.body[0].y += csnake.directionY;

          GetTile(csnake.body[0]).componentTile().tag = Tags.Snake;
        }
      }

      // draw
      Render(dt);
    }

    void Render(float dt = 0)
    {
      foreach (var tile in tiles)
      {
        var ctile    = tile.componentTile();
        var renderer = tile.GetMono<SpriteRenderer>();

        switch (ctile.tag)
        {
          case Tags.Empty:
          {
            renderer.color = freeColor;
          }
            break;
          case Tags.Food:
          {
            renderer.color = foodColor;
          }
            break;
          case Tags.Obstacle:
          {
            renderer.color = obstacleColor;
          }
            break;
          case Tags.Snake:
          {
            if (blinkTime > 0)
            {
              renderer.color =  snakeColorBlink;
              blinkTime      -= dt;
            }
            else renderer.color = snakeColor;
          }
            break;
        }
      }
    }
  }
}
