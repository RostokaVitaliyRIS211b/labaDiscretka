RenderWindow MainWindow = new(new VideoMode(1280, 720), "Lab1");
while(MainWindow.IsOpen)
{
    MainWindow.Clear(Color.White);
    MainWindow.DispatchEvents();
    MainWindow.Display();
}