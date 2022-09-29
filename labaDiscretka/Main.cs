using Sets;
using Textboxes;

List<CircleTextbox> circles = new();
List<Set> sets = new();
List<Sprite> buttons = new();

CircleTextbox? catching = null;

Textbox universum = new();

buttons.Add(new Sprite
{
    Position = new Vector2f(0, 0),
    Texture = new(new Image(Directory.GetCurrentDirectory()+"/addSet.png"))
});

RenderWindow MainWindow = new(new VideoMode(1280, 720), "Lab1");

MainWindow.MouseButtonPressed+=MouseButtonPressed;
MainWindow.MouseMoved+=MouseMoved;
MainWindow.MouseButtonReleased+=MouseButtonReleased;

universum.set_size_character_text(32);
universum.set_string("Universum");
universum.set_size_rect(1280, 720);
universum.set_pos(640, 360);
universum.set_Fill_color_rect(new Color(181,230,29));
universum.set_color_text(Color.Black);


while(MainWindow.IsOpen)
{
    MainWindow.Clear(Color.White);
    MainWindow.DispatchEvents();
    MainWindow.Draw(universum);
    foreach (Sprite sprite in buttons)
        MainWindow.Draw(sprite);
    foreach (CircleTextbox circle in circles)
        MainWindow.Draw(circle);
    if(catching is not null)
        MainWindow.Draw(catching);
    MainWindow.Display();//255 201 14
}
void MouseButtonPressed(object? sender,MouseButtonEventArgs? e)
{
   if(e.Button==Mouse.Button.Left && buttons[0].GetGlobalBounds().Contains(e.X,e.Y) && catching is null)
   {
        Set set = new();
        sets.Add(set);
        CircleTextbox circle = new();
        circle.SetString(set.Name);
        circle.SetRadius(40);
        circle.SetPosition(640,320);
        circle.SetCharacterSize(12);
        circle.SetFillColorCircle(new Color(255, 201, 14));
        circle.SetFillColorText(Color.Black);
        circle.SetOutlineColorCircle(Color.Black);
        circle.SetOutlineThicknessCircle(2);
        circles.Add(circle);
        return;
   }
   if(catching is null)
   {
        foreach (CircleTextbox circle in circles)
        {
            if (circle.Contains(e.X, e.Y))
            {
                catching = circle;
                return;
            }
        }
    }
   
}
void MouseMoved(object? sender, MouseMoveEventArgs? e)
{
    catching?.SetPosition(e.X,e.Y);
}
void MouseButtonReleased(object? sender, MouseButtonEventArgs? e)
{
    catching = null;
}