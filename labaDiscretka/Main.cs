using Sets;
using System.Text;
using System.Xml.Linq;
using Textboxes;

StringBuilder text = new();

int code = -1;

List<CircleTextbox> circles = new();
List<Set> sets = new();
List<Sprite> buttons = new();
List<Textbox> textboxes = new();
List<Textbox> operations = new();

InitTextboxes();

CircleTextbox? mutable = null;
CircleTextbox? catching = null;

int[] operIndexes = new int[2]; 

Clock clock = new();
Textbox universum = new();
Textbox active = new();

active.set_color_text(Color.Black);
active.set_Fill_color_rect(Color.White);
active.set_outline_color_rect(Color.Black);
active.set_outline_thickness_rect(1);
active.set_size_rect(500, 50);
active.set_size_character_text(16);
active.set_pos(640, 360);

Textbox active2 = new(in active);
active2.set_pos(640, 310);
Textbox exception = new();
exception.Copy(active);
exception.set_size_rect(550, 120);
exception.set_size_character_text(24);

bool isSpisokDraw = false;
bool isExceptionDraw = false;
bool isActiveDraw = false;
bool isOperationsDraw = false;

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

MainWindow.SetFramerateLimit(60);

while(MainWindow.IsOpen)
{
    MainWindow.Clear(Color.White);
    MainWindow.Draw(universum);

    try
    {
        MainWindow.DispatchEvents();
    }
    catch(Exception e)
    {
        isExceptionDraw = true;
        exception.set_string(e.Message);
    }

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
    if(isActiveDraw)
    {
        Vector2f t = new(active.GetText().GetGlobalBounds().Left+active.GetText().GetGlobalBounds().Width+1, 352);
        Vector2f d = new(active.GetText().GetGlobalBounds().Left+active.GetText().GetGlobalBounds().Width+1, 367.8f);
        MainWindow.Draw(active);
        MainWindow.Draw(active2);
        if (clock.ElapsedTime.AsSeconds()>1 && clock.ElapsedTime.AsSeconds()<2)
        {
            MainWindow.Draw(new Vertex[] { new Vertex(t, Color.Black), new Vertex(d, Color.Black) }, PrimitiveType.Lines, RenderStates.Default);
        }
        else if(clock.ElapsedTime.AsSeconds()>2)
            clock.Restart();
    }
    if(isOperationsDraw)
    {
        for (int i = 0; i<operations.Count; ++i)
        {
            if (i>0)
            {
                Vector2f rty = operations[i - 1].get_position();
                operations[i].set_pos(new Vector2f(rty.X, rty.Y + 20));
            }
            MainWindow.Draw(operations[i]);
        }
    }
    if(isExceptionDraw)
    {
        MainWindow.Draw(exception);
    }
    MainWindow.Display();//255 201 14
}
void MouseButtonPressed(object? sender,MouseButtonEventArgs? e)
{
    isExceptionDraw = false;
    active.set_string("");
    if(isSpisokDraw && e.Button == Mouse.Button.Left)
    {
        int n = 0;
        bool cliked = false;
        clock.Restart();
        foreach (Textbox textbox in textboxes)
        {
            n++;
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
                        active2.set_string("Write integer number for add it to set " + set.Name);
                    }
                    else
                    {
                        active2.set_string("Write integer number for add it to set " + Universum.name);
                    }
                    break;
                case 2:
                    if (mutable is not null)
                    {
                        active2.set_string("Write integer number for delete it from set " + set.Name);
                    }
                    else
                    {
                        active2.set_string("Write integer number for delete it from set " + Universum.name);
                    }
                    break;
                case 3:
                    RemoveAll(set, mutable is null);
                    isActiveDraw = false;
                    break;
                case 4:
                    if (mutable is not null)
                    {
                        active2.set_string("Write two integer number to set bounds " + set.Name);
                    }
                    else
                    {
                        active2.set_string("Write two integer number to set bounds " + Universum.name);
                    }
                    break;
                case 5:
                    if (mutable is not null)
                    {
                        active2.set_string("Write name of set " + set.Name);
                    }
                    else
                    {
                        active2.set_string(new string("Write name of set ") + Universum.name);
                    }
                    break;
                case 6:
                    if(mutable is not null)
                    {
                        active2.set_string("All elements of set "+set.Name);
                        active.set_string(set.ToString() ?? "GAY");
                    }
                    else
                    {
                        string s = "{ ";
                        foreach (int x in Universum.elements)
                        {
                            s+=x+" ";
                        }
                        s+="}";
                        active2.set_string("All elements of set "+Universum.name);
                        active.set_string(s ?? "GAY");
                    }
                    break;
                case 7:
                    Set set1 = set.Addition();
                    CircleTextbox circle = new();
                    circle.SetRadius(40);
                    circle.SetPosition(640, 320);
                    circle.SetCharacterSize(12);
                    circle.SetFillColorCircle(new Color(255, 201, 14));
                    circle.SetFillColorText(Color.Black);
                    circle.SetOutlineColorCircle(Color.Black);
                    circle.SetOutlineThicknessCircle(2);
                    if(mutable is not null)
                    {
                        set1.Name="!"+mutable.GetString();
                        circle.SetString(set.Name);
                    }
                    else
                    {
                        set1 = new();
                        circle.SetString("Empty set");
                    }
                    circles.Add(circle);
                    sets.Add(set1);
                    mutable = null;
                    isActiveDraw = false;
                    break;
                default:
                    isActiveDraw = false;
                    mutable = null;
                    break;

            }
        }
        else
        {
            isActiveDraw = false;
            mutable = null;
        }

        isSpisokDraw = false;

        return;
    }
    if(isOperationsDraw && e.Button == Mouse.Button.Left)
    {
        int n = 0;
        bool cliked = false;
        clock.Restart();
        foreach (Textbox textbox in operations)
        {
            n++;
            if (textbox.contains(e.X, e.Y))
            {
                cliked = true;
                break;
            }
        }
        if(cliked)
        {
            Set set = new();
            CircleTextbox circle = new();
            circle.SetRadius(40);
            circle.SetPosition(640, 320);
            circle.SetCharacterSize(12);
            circle.SetFillColorCircle(new Color(255, 201, 14));
            circle.SetFillColorText(Color.Black);
            circle.SetOutlineColorCircle(Color.Black);
            circle.SetOutlineThicknessCircle(2);
            switch (n)
            {
                case 1:
                    set = Set.Unification(sets[operIndexes[0]], sets[operIndexes[1]]);
                    set.Name = sets[operIndexes[0]].Name+" || "+sets[operIndexes[1]].Name;
                    circle.SetString(set.Name);
                    break;
                case 2:
                    set = Set.Crossing(sets[operIndexes[0]], sets[operIndexes[1]]);
                    set.Name = sets[operIndexes[0]].Name+" && "+sets[operIndexes[1]].Name;
                    circle.SetString(set.Name);
                    break;
                case 3:
                    set = Set.Difference(sets[operIndexes[0]], sets[operIndexes[1]]);
                    set.Name = sets[operIndexes[0]].Name+" / "+sets[operIndexes[1]].Name;
                    circle.SetString(set.Name);
                    break;
                case 4:
                    set = Set.SymmDifference(sets[operIndexes[0]], sets[operIndexes[1]]);
                    set.Name = sets[operIndexes[0]].Name+" △ "+sets[operIndexes[1]].Name;
                    circle.SetString(set.Name);
                    break;
            }
            
            circles.Add(circle);
            sets.Add(set);
        }
        catching = null;
        isOperationsDraw = false;
       
    }
    isOperationsDraw = false;
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
    if (catching is null && isOperationsDraw)
    {
        foreach (Textbox textbox in operations)
        {
            if (textbox.contains(e.X, e.Y))
            {
                textbox.set_Fill_color_rect(new Color(157, 250, 234));
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
    if(catching is not null)
    {
        foreach (CircleTextbox circle in circles)
        {
            if (Dlina(catching.GetPosition(), circle.GetPosition())<=catching.GetRadius()+circle.GetRadius() && catching!=circle)
            {
                operIndexes[0]=circles.IndexOf(catching);
                operIndexes[1]=circles.IndexOf(circle);
                isOperationsDraw = true;
                operations[0].set_pos(e.X, e.Y);
                break;
            }
        }
    }
   
    catching = null;
}
void KeyPressed(object? sender, KeyEventArgs? e)
{
    isExceptionDraw = false;
    if(e.Code==Keyboard.Key.Escape)
    {
        MainWindow.Close();
        return;
    }
    if(e.Code == Keyboard.Key.Backspace)
    {
        active.set_string(active.get_string().Remove(active.get_string().Length-1));
        return;
    }
    if(e.Code!=Keyboard.Key.Enter && isActiveDraw)
    {
        char ee = WhatCharItIs(e.Code);
        if(ee!='+')
            active.set_string(active.get_string()+ee);
        return;
    }
  
    if(e.Code == Keyboard.Key.Enter && isActiveDraw)
    {
        isActiveDraw = false;
        int index = mutable is not null ? circles.IndexOf(mutable) : -1;
        Set set = index!=-1 ? sets[index] : new();
        string message = IsItException(active.get_string(), code,mutable is null);

        if (message!="NO")
            code=-1;

        switch (code)
        {
            case 1:
                AddElem(set, active.get_string(),mutable is null);
                break;

            case 2:
                RemoveElem(set, active.get_string(), mutable is null);
                break;

            case 4:
                SetBounds(set, active.get_string(), mutable is null);
                break;

            case 5:
                Rename(set,active.get_string(),mutable is null);
                break;

            default:
                throw new Exception(message);
        }
        isActiveDraw = false;
        mutable = null;
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
    textbox.set_size_rect(90, 20);
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
    textbox = new();
    textbox.Copy(textboxes[0]);
    textbox.set_string("Show Elements");
    textbox.set_pos(40, 110);
    textboxes.Add(textbox);
    textbox = new();
    textbox.Copy(textboxes[0]);
    textbox.set_string(" ! ");
    textbox.set_pos(40, 130);
    textboxes.Add(textbox);
    textbox = new();
    textbox.Copy(textboxes[0]);
    textbox.set_string(" || ");
    textbox.set_pos(40, 10);
    operations.Add(textbox);
    textbox = new();
    textbox.Copy(textboxes[0]);
    textbox.set_string(" && ");
    textbox.set_pos(40, 10);
    operations.Add(textbox);
    textbox = new();
    textbox.Copy(textboxes[0]);
    textbox.set_string(" / ");
    textbox.set_pos(40, 10);
    operations.Add(textbox);
    textbox = new();
    textbox.Copy(textboxes[0]);
    textbox.set_string(" △ ");
    textbox.set_pos(40, 10);
    operations.Add(textbox);
}
void AddElem(Set set,string item,bool un = false)
{
    int item1;
    if (!Int32.TryParse(item, out item1))
        throw new Exception("Exception in Addelem with element");
    if (!un)
    {
        set.Add(item1);
    }
    else
    {
        if(!Universum.elements.Contains(item1))
            Universum.elements.Add(item1);
    }
}
void RemoveElem(Set set, string item, bool un = false)
{
    if (!un)
    {
        int item1;
        if (Int32.TryParse(item, out item1))
            set.RemoveElem(item1);
    }
    else
    {
        int item1;
        if (Int32.TryParse(item, out item1))
            Universum.elements.Remove(item1);
    }
}
void RemoveAll(Set set, bool un = false)
{
    if (!un)
    {
        set.Name = "Empty set";
        set.Clear();
    }
    else
    {
        Universum.elements.Clear();
        circles.Clear();
        sets.Clear();
    }
}
void SetBounds(Set set, string item, bool un = false)
{
    string[] items = item.Split(' ');
    int item1, item2;

    if (items.Length<1)
        throw new Exception("Exception in SetBounds with count of elements");

    bool fl1 = Int32.TryParse(items[0], out item1);
    bool fl2 = Int32.TryParse(items[1], out item2);

    if(!fl1 || !fl2)
        throw new Exception("Exception in SetBounds with one of element");

    if (item1>=item2)
        throw new Exception("Lower >= Higher");

    if (!un)
    {
        set.SetBounds(item1, item2);
    }
    else
    {
        Universum.elements = new(item2-item1);
        for (int i = item1; i<item2; ++i)
        {
            Universum.elements.Add(i);
        }
        foreach (Set set1 in sets)
        {
            set1.AccordingToU();
        }
    }
}
void Rename( Set set,string name, bool un = false)
{
    if(!un)
    {
        set.Name=name;
        circles[sets.IndexOf(set)].SetString(name);
    }
    else
    {
        Universum.name=name;
        universum.set_string(name);
    }
}
string IsItException(string s1,int code,bool un = false)
{
    string s = "NO";
    int item, item1, item2;
    if (code == 1)
    {
        bool fl = Int32.TryParse(s1, out item);
        if(!fl)
        {
            if (s1.Contains('.'))
                s="Element must be a integer";
            else if (!isThisInt(s1))
                s="Element contains illegal symbol";
            else
                s="Element out of range type integer";
        }
        if (!un && !Universum.elements.Contains(item))
            s="Element does not exists in Universum";
    }
    if(code == 4)
    {
        string[] items = s1.Split(' ');
        if(items.Length<2)
        {
            s = "You must write two numbers for set bounds";
        }
        else
        {
            bool fl1 = Int32.TryParse(items[0], out item1);
            bool fl2 = Int32.TryParse(items[1], out item2);
            if(!fl1)
            {
                if(items[0].Contains('.'))
                    s="Left bound must be a integer";
                else if (!isThisInt(items[0]))
                    s="Left bound contains illegal symbol";
                else
                    s="Left bound out of range of integer type";
            }
            else if(!un && !Universum.elements.Contains(item1))
            {
                s="Left bound does not exists in Universum";
            }
            else if(!fl2)
            {
                if (items[1].Contains('.'))
                    s="Right bound must be a integer";
                else if (!isThisInt(items[1]))
                    s="Right bound contains illegal symbol";
                else
                    s="Right bound out of range of integer type";
            }
            else if (!un && !Universum.elements.Contains(item1))
            {
                s="Right bound does not exists in Universum";
            }
            else if(item1>=item2)
            {
                s="Left bound must be less then Right bound";
            }
        }
    }
    return s;
}
bool isThisInt(string s)
{
    bool f = true;
    string numbers = "0123456789";
    for(int i=0;i<s.Length && f;++i)
    {
        if (!numbers.Contains(s[i]) ||(i==0 && s[i]=='0'))
            f = false;
    }
    return f;
}
char WhatCharItIs(Keyboard.Key code)
{
    char ch='0';
    string gay = "abcdefghijklmnopqrstuvwxyz0123456789";
    
    if ((int)code<36)
    {
        ch = Keyboard.IsKeyPressed(Keyboard.Key.LShift)&&(int)(code)<25 ? (char)((int)gay[(int)code]-32): gay[(int)code];
    }
    else if ((int)code==57)
        ch= ' ';
    else if((int)code==56 || (int)code==68)
        ch='-';
    else
        ch='+';
    return ch;
}
double Dlina(Vector2f pos1,Vector2f pos2)
{
    return Math.Sqrt(Math.Pow(pos1.X-pos2.X,2)+Math.Pow(pos1.Y-pos2.Y, 2));
}