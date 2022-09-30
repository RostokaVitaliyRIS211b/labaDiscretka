using Sets;
using Textboxes;

List<CircleTextbox> circles = new();
List<Set> sets = new();
List<Sprite> buttons = new();
List<Textbox> textboxes = new();

InitTextboxes();

CircleTextbox? mutable = null;
CircleTextbox? catching = null;

Textbox universum = new();

bool isSpisokDraw = false;

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

    if(isSpisokDraw)
    {
       for(int i=0;i<textboxes.Count;++i)
       {
            if (i>0)
            {
                Vector2f rty = textboxes[i - 1].get_position();
                textboxes[i].set_pos(new Vector2f(rty.X, rty.Y + 20));
            }
            MainWindow.Draw(textboxes[i]);
       }
    }
    
    MainWindow.Display();//255 201 14
}
void MouseButtonPressed(object? sender,MouseButtonEventArgs? e)
{   if(isSpisokDraw && e.Button == Mouse.Button.Left)
    {

        isSpisokDraw = false;
        mutable = null;
        return;
    }
    isSpisokDraw = false;
    mutable = null;
    if(e.Button == Mouse.Button.Left && buttons[0].GetGlobalBounds().Contains(e.X,e.Y) && catching is null)
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
    if(e.Button == Mouse.Button.Left && catching is null)
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
    if(e.Button == Mouse.Button.Right && catching is null)
    {
        foreach(CircleTextbox circle in circles)
        {
            if(circle.Contains(e.X, e.Y))
            {
                textboxes[0].set_pos(e.X+40,e.Y+10);
                mutable = circle;
                isSpisokDraw = true;
                return;
            }
        }
    }
    if(e.Button == Mouse.Button.Right && catching is null)
    {
        textboxes[0].set_pos(e.X + 40, e.Y + 10);
        mutable = null;
        isSpisokDraw = true;
    }
   
}
void MouseMoved(object? sender, MouseMoveEventArgs? e)
{
    catching?.SetPosition(e.X,e.Y);
    if(catching is null && isSpisokDraw)
    {
        foreach(Textbox textbox in textboxes)
        {
            if(textbox.contains(e.X,e.Y))
            {
                textbox.set_Fill_color_rect(new Color(157,250,234));
            }
            else
            {
                textbox.set_Fill_color_rect(Color.White);
            }
        }
    }
}
void MouseButtonReleased(object? sender, MouseButtonEventArgs? e)
{
    catching = null;
}
void InitTextboxes()
{
    Textbox textbox = new Textbox();
    textbox.set_color_text(Color.Black);
    textbox.set_string("Add element");
    textbox.set_Fill_color_rect(Color.White);
    textbox.set_outline_color_rect(Color.Black);
    textbox.set_outline_thickness_rect(1);
    textbox.set_size_character_text(12);
    textbox.set_size_rect(80, 20);
    textbox.set_pos(40, 10);
    textboxes.Add(textbox);
    textbox = new();
    textbox.Copy(textboxes[0]);
    textbox.set_string("Remove elem");
    textbox.set_pos(40, 30);
    textboxes.Add(textbox);
    textbox = new();
    textbox.Copy(textboxes[0]);
    textbox.set_string("Remove elem");
    textbox.set_pos(40, 50);
    textboxes.Add(textbox);
    textbox = new();
    textbox.Copy(textboxes[0]);
    textbox.set_string("Remove All");
    textbox.set_pos(40, 70);
    textboxes.Add(textbox);
    textbox = new();
    textbox.Copy(textboxes[0]);
    textbox.set_string("Set Bounds");
    textbox.set_pos(40, 90);
    textboxes.Add(textbox);
    textbox = new();
    textbox.Copy(textboxes[0]);
    textbox.set_string("Rename");
    textbox.set_pos(40, 110);
}
void AddElem(Set set,bool un = false)
{
    if(!un)
    {

    }
    else
    {

    }
}
void RemoveElem(Set set, bool un = false)
{

}
void RemoveAll(Set set, bool un = false)
{

}
void Rename(CircleTextbox textbox, Set set, bool un = false)
{

}
void SetBounds(Set set, bool un = false)
{

}