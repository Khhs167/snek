using Raylib_cs;

class Program {
  public static void Main(string[] args){

    float speed = 10f; // Low == slow
    int score = 0;
    int maxscore = 0;

    bool smooth = false;

    Console.WriteLine("snek by khhs");

    string datapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    Console.WriteLine("Data path is located at " + datapath);
    if(!Directory.Exists(datapath + "/snek"))
      Directory.CreateDirectory(datapath + "/snek");
    if(File.Exists(datapath + "/snek/score.txt")){
      string[] scoreData = File.ReadAllLines(datapath + "/snek/score.txt");
      maxscore = int.Parse(scoreData[0]);
      score = int.Parse(scoreData[1]);
    }

    List<Position> snek = new List<Position>();
    List<Position> oldsnek = new List<Position>();
    Queue<Position> inputs = new Queue<Position>(); 
    Position direction = new Position(1, 0);
    Position food = new Position(Random.Shared.Next(0, 80), Random.Shared.Next(0, 80));

    Raylib.InitWindow(800, 800, "Snek");
    Raylib.SetTargetFPS(120);

    void StartGame() {
      direction = new Position(1, 0);
      inputs.Clear();
      snek.Clear();
      snek.Add(new Position(40, 40));
      snek.Add(new Position(39, 40));
      snek.Add(new Position(38, 40));
    }

    void GameOver() {
      snek.Clear();
      string datapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      if(!Directory.Exists(datapath + "/snek"))
        Directory.CreateDirectory(datapath + "/snek");
      File.WriteAllText(datapath + "/snek/score.txt", maxscore.ToString() + "\n" + score.ToString());
    }

    float movetimer = 0;
    while(!Raylib.WindowShouldClose()){

      if(Raylib.IsKeyPressed(KeyboardKey.KEY_O)){
        smooth = !smooth;
      }

      if (inputs.Count > 0){
        if(Raylib.IsKeyPressed(KeyboardKey.KEY_W)){
          if(inputs.Last().y != 1)
            inputs.Enqueue(new Position(0, -1));
        }

        if(Raylib.IsKeyPressed(KeyboardKey.KEY_S)){
          if(inputs.Last().y != -1)
            inputs.Enqueue(new Position(0, 1));
        }

        if(Raylib.IsKeyPressed(KeyboardKey.KEY_A)){
          if(inputs.Last().x == 1)
            inputs.Enqueue(new Position(-1, 0));
        }

        if(Raylib.IsKeyPressed(KeyboardKey.KEY_D)){
          if(inputs.Last().x != -1)
            inputs.Enqueue(new Position(1, 0));
        }
      } else {
        if(Raylib.IsKeyPressed(KeyboardKey.KEY_W)){
          if(direction.y != 1)
            inputs.Enqueue(new Position(0, -1));
        }

        if(Raylib.IsKeyPressed(KeyboardKey.KEY_S)){
          if(direction.y != -1)
            inputs.Enqueue(new Position(0, 1));
        }

        if(Raylib.IsKeyPressed(KeyboardKey.KEY_A)){
          if(direction.x != 1)
            inputs.Enqueue(new Position(-1, 0));
        }

        if(Raylib.IsKeyPressed(KeyboardKey.KEY_D)){
          if(direction.x != -1)
            inputs.Enqueue(new Position(1, 0));
        }
      }

      if(Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) && snek.Count == 0){
        StartGame();
      }


      movetimer += Raylib.GetFrameTime();

      if(movetimer > 1.0f / speed && snek.Count > 0){

        oldsnek.Clear();
        oldsnek.AddRange(snek);


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
          speed *= 1.01f;
        }

        for(int i = 1; i < snek.Count; i++){
          if(snek[0].x == snek[i].x && snek[0].y == snek[i].y){
            GameOver();
            goto loopend;
          }
        }

        if(snek[0].x < 0 || snek[0].x > 80 || snek[0].y < 0 || snek[0].y > 80){
          GameOver();
          goto loopend;
        }
        
      }



      Raylib.BeginDrawing();
      Raylib.ClearBackground(Color.BLACK);

      if(snek.Count > 0){

        Raylib.DrawText($"Score: {score}", 5, 5, 10, Color.DARKGRAY);
        Raylib.DrawText($"High: {maxscore}", 5, 20, 10, Color.DARKGRAY);

        for(int i = 0; i < snek.Count; i++){

          float Lerp(float firstFloat, float secondFloat, float by)
          {
              return firstFloat * (1 - by) + secondFloat * by;
          }


          float x, y;

          if(i < oldsnek.Count && smooth){
            Position a = oldsnek[i];
            Position b = snek[i];
            x = Lerp(a.x, b.x, (movetimer / (1f / speed)));
            y = Lerp(a.y, b.y, (movetimer / (1f / speed)));
          } else{
            x = snek[i].x;
            y = snek[i].y;
          }

          if(i == 0){
            Raylib.DrawRectangle((int)(x * 10), (int)(y * 10), 10, 10, Color.LIME);
          } else{
            Raylib.DrawRectangle((int)(x * 10), (int)(y * 10), 10, 10, Color.DARKGREEN);
          }
          
        }

        Raylib.DrawRectangle(food.x * 10, food.y * 10, 10, 10, Color.RED);
      } else{
        Raylib.DrawText("PRESS SPACE TO START", 5, 5, 20, Color.GREEN);
        Raylib.DrawText($"SCORE: {score}", 5, 30, 20, Color.GREEN);
        Raylib.DrawText($"HIGH: {maxscore}", 5, 55, 20, Color.GREEN);
      }

      Raylib.EndDrawing();

      loopend:;

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
