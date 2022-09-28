using Raylib_cs;

class Program {
  public static void Main(string[] args){
    Console.WriteLine("snek");

    List<Position> snek = new List<Position>();
    Queue<Position> inputs = new Queue<Position>(); 
    Position direction = new Position(1, 0);
    Position food = new Position(Random.Shared.Next(0, 80), Random.Shared.Next(0, 80));

    Raylib.InitWindow(800, 800, "Snek");
    Raylib.SetTargetFPS(60);

    const float speed = 10f; // Low == slow
    int score = 0;
    int maxscore = 0;

    void StartGame() {
      snek.Clear();
      snek.Add(new Position(40, 40));
      snek.Add(new Position(39, 40));
      snek.Add(new Position(38, 40));
    }

    float movetimer = 0;
    while(!Raylib.WindowShouldClose()){


      if(Raylib.IsKeyPressed(KeyboardKey.KEY_W)){
        inputs.Enqueue(new Position(0, -1));
      }

      if(Raylib.IsKeyPressed(KeyboardKey.KEY_S)){
        inputs.Enqueue(new Position(0, 1));
      }

      if(Raylib.IsKeyPressed(KeyboardKey.KEY_A)){
        inputs.Enqueue(new Position(-1, 0));
      }

      if(Raylib.IsKeyPressed(KeyboardKey.KEY_D)){
        inputs.Enqueue(new Position(1, 0));
      }

      if(Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) && snek.Count == 0){
        StartGame();
      }


      movetimer += Raylib.GetFrameTime();

      if(movetimer > 1.0f / speed && snek.Count > 0){

        score = snek.Count - 3;

        if(score > maxscore)
          maxscore = score;

        if(inputs.Count > 0){
          direction = inputs.Dequeue();
        }

        for(int i = snek.Count - 1; i > 0; i--){
          snek[i] = snek[i - 1];
        }
        Position head = snek[0];
        head.x += direction.x;
        head.y += direction.y;
        snek[0] = head;
        movetimer = 0;

        if(snek[0].x == food.x && snek[0].y == food.y){
          food = new Position(Random.Shared.Next(0, 80), Random.Shared.Next(0, 80));
          snek.Add(snek[snek.Count - 1]);
        }

        for(int i = 1; i < snek.Count; i++){
          if(snek[0].x == snek[i].x && snek[0].y == snek[i].y){
            snek.Clear();
          }
        }

        if(snek[0].x < 0 || snek[0].x > 80 || snek[0].y < 0 || snek[0].y > 80){
          snek.Clear();
        }
        
      }



      Raylib.BeginDrawing();
      Raylib.ClearBackground(Color.BLACK);

      if(snek.Count > 0){

        Raylib.DrawText($"Score: {score}", 5, 5, 10, Color.DARKGRAY);
        Raylib.DrawText($"High: {maxscore}", 5, 20, 10, Color.DARKGRAY);

        for(int i = 0; i < snek.Count; i++){
          Position c = snek[i];
          if(i == 0){
            Raylib.DrawRectangle(c.x * 10, c.y * 10, 10, 10, Color.LIME);
          } else{
            Raylib.DrawRectangle(c.x * 10, c.y * 10, 10, 10, Color.DARKGREEN);
          }
          
        }

        Raylib.DrawRectangle(food.x * 10, food.y * 10, 10, 10, Color.RED);
      } else{
        Raylib.DrawText("PRESS SPACE TO START", 5, 5, 20, Color.GREEN);
        Raylib.DrawText($"SCORE: {score}", 5, 30, 20, Color.GREEN);
        Raylib.DrawText($"HIGH: {maxscore}", 5, 55, 20, Color.GREEN);
      }

      Raylib.EndDrawing();

    }

  }
}

struct Position {

  public Position(int x, int y){
    this.x = x;
    this.y = y;
  }

  public int x, y;
};
