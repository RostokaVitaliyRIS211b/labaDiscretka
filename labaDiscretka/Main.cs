using Sets;
using System.Text;
using Textboxes;

StringBuilder text = new();

int code = -1;

List<CircleTextbox> circles = new();
List<Set> sets = new();
List<Sprite> buttons = new();
List<Textbox> textboxes = new();

InitTextboxes();

CircleTextbox? mutable = null;
CircleTextbox? catching = null;

Textbox universum = new();

Textbox active = new();
active.set_color_text(Color.Black);
active.set_Fill_color_rect(Color.White);
active.set_outline_color_rect(Color.Black);
active.set_outline_thickness_rect(1);
active.set_size_rect(150, 50);
active.set_size_character_text(16);
active.set_pos(565, 335);

Textbox active2 = new(in active);
active2.set_pos(565, 285);

bool isSpisokDraw = false;

bool isActiveDraw = false;  

buttons.Add(new Sprite
{
    Position = new Vector2f(0, 0),
    Texture = new(new Image(Directory.GetCurrentDirectory()+"/addSet.png"))
});

RenderWindow MainWindow = new(new VideoMode(1280, 720), "Lab1");

MainWindow.MouseButtonPressed += MouseButtonPressed;
MainWindow.MouseMoved += MouseMoved;
MainWindow.MouseButtonReleased += MouseButtonReleased;
MainWindow.KeyPressed += KeyPressed;

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
{   
    if(isSpisokDraw && e.Button == Mouse.Button.Left)
    {
        int n = 0;
        bool cliked = false;

        foreach(Textbox textbox in textboxes)
        {
            ++n;
            if(textbox.contains(e.X,e.Y))
            {
                cliked = true;
                break;
            }
        }

        int index = mutable is not null ? circles.IndexOf(mutable) : -1;
        Set set = index!=-1 ? sets[index] : new();

        text.Clear();
        active.set_string(text.ToString());
        isActiveDraw = true;
        code = n;

        if(cliked)
        {
            switch (n)
            {
                case 1:
                    if (mutable is not null)
                    {
                        active2.set_string("Введите целое число для добавления в множество " + set.Name);
                    }
                    else
                    {
                        active2.set_string("Введите целое число для добавления в множество " + Universum.name);
                    }
                    break;
                case 2:
                    if (mutable is not null)
                    {
                        active2.set_string("Введите целое число для удаления из в множества " + set.Name);
                    }
                    else
                    {
                        active2.set_string("Введите целое число для удаления из в множества " + Universum.name);
                    }
                    break;
                case 3:
                    
                    break;
                case 4:
                    if (mutable is not null)
                    {
                        active2.set_string("Введите два целых числа для задания множества " + set.Name);
                    }
                    else
                    {
                        active2.set_string("Введите два целых числа для задания множества " + Universum.name);
                    }
                    break;
                case 5:
                    if (mutable is not null)
                    {
                        active2.set_string("Введите имя множества " + set.Name);
                    }
                    else
                    {
                        active2.set_string("Введите имя множества " + Universum.name);
                    }
                    break;
            }
        }

        isSpisokDraw = false;

        return;
    }

    isActiveDraw = false;
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
void KeyPressed(object? sender, KeyEventArgs? e)
{
    if(e.Code==Keyboard.Key.Escape)
    {
        MainWindow.Close();
    }
    else if(e.Code == Keyboard.Key.Enter && isActiveDraw)
    {
        int index = mutable is not null ? circles.IndexOf(mutable) : -1;
        Set set = index!=-1 ? sets[index] : new();
        int item;
        Int32.TryParse(active.get_string(), out item);


        switch (code)
        {
            case 1:
                AddElem(set, item,mutable is null);
                break;
            case 2:
                RemoveElem(set, item, mutable is null);
                break;
            case 3:
                RemoveAll(set, mutable is null);
                break;
            case 4:
                SetBounds(item1,item2,set,mutable is null);
                break;
            case 5:
                Rename(set, mutable is null);
                break;
            default: 
                break;
        }
    }
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
    textbox.set_string("Remove All");
    textbox.set_pos(40, 50);
    textboxes.Add(textbox);
    textbox = new();
    textbox.Copy(textboxes[0]);
    textbox.set_string("Set Bounds");
    textbox.set_pos(40, 70);
    textboxes.Add(textbox);
    textbox = new();
    textbox.Copy(textboxes[0]);
    textbox.set_string("Rename");
    textbox.set_pos(40, 90);
    textboxes.Add(textbox);
}
void AddElem(Set set,int item,bool un = false)
{
   
}
void RemoveElem(Set set,int item ,bool un = false)
{
    
}
void RemoveAll(Set set, bool un = false)
{
    
}
void SetBounds(int item1, int item2,Set set, bool un = false)
{
    
}
void Rename( Set set, bool un = false)
{
    
}
string IsItException(string s1,int code)
{
    string s = "NO";
    int item, item1, item2;

    if ((!Int32.TryParse(active.get_string(), out item) || !Universum.elements.Contains(item)) && code == 1)
        code = -1;

    string[] its = active.get_string().Split(' ');
    string it1 = its[0];
    string? it2 = its.Length>1 ? its[1] : null;

    bool fl1 = !Int32.TryParse(it1, out item1);
    bool fl2 = !Int32.TryParse(it2, out item2);

    if ((fl1 || fl2 || item1>=item2) && code == 4)
        code=-1;
    return s;
}
