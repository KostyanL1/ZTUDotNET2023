using System;
abstract class MusicalInstrument
{
    protected string Name;
    protected string Characteristics;
    public MusicalInstrument()
    {
        Name = "Unknown";
        Characteristics = "Unknown";
    }
    public MusicalInstrument(string name, string characteristics)
    {
        Name = name;
        Characteristics = characteristics;
    }
    public MusicalInstrument(MusicalInstrument other)
    {
        Name = other.Name;
        Characteristics = other.Characteristics;
    }
    public abstract void Sound();
    public abstract void Show();
    public abstract void Desc();
    public abstract void History();
    public void ShowInfo()
    {
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Characteristics: {Characteristics}");
    }
}
class Violin : MusicalInstrument
{
    public Violin() : base() { }
    public Violin(string name, string characteristics) : base(name, characteristics) { }
    public Violin(Violin other) : base(other) { }
    public override void Sound()
    {
        Console.WriteLine("Violin sound");
    }
    public override void Show()
    {
        Console.WriteLine("Violin");
    }
    public override void Desc()
    {
        Console.WriteLine("A string instrument with four strings.");
    }
    public override void History()
    {
        Console.WriteLine("Violin was invented in the early 16th century.");
    }
}
class Trombone : MusicalInstrument
{
    public Trombone() : base() { }
    public Trombone(string name, string characteristics) : base(name, characteristics) { }
    public Trombone(Trombone other) : base(other) { }
    public override void Sound()
    {
        Console.WriteLine("Trombone sound");
    }
    public override void Show()
    {
        Console.WriteLine("Trombone");
    }
    public override void Desc()
    {
        Console.WriteLine("A brass instrument with a sliding tube.");
    }
    public override void History()
    {
        Console.WriteLine("Trombone has a long history, dating back to the Renaissance.");
    }
}
class Ukulele : MusicalInstrument
{
    public Ukulele() : base() { }
    public Ukulele(string name, string characteristics) : base(name, characteristics) { }
    public Ukulele(Ukulele other) : base(other) { }
    public override void Sound()
    {
        Console.WriteLine("Ukulele sound");
    }
    public override void Show()
    {
        Console.WriteLine("Ukulele");
    }
    public override void Desc()
    {
        Console.WriteLine("A small, four-stringed instrument.");
    }
    public override void History()
    {
        Console.WriteLine("Ukulele originated in the 19th century in Hawaii.");
    }
}
class Cello : MusicalInstrument
{
    public Cello() : base() { }
    public Cello(string name, string characteristics) : base(name, characteristics) { }
    public Cello(Cello other) : base(other) { }
    public override void Sound()
    {
        Console.WriteLine("Cello sound");
    }
    public override void Show()
    {
        Console.WriteLine("Cello");
    }
    public override void Desc()
    {
        Console.WriteLine("A large string instrument, similar to the violin.");
    }
    public override void History()
    {
        Console.WriteLine("Cello has a history dating back to the 16th century.");
    }
}
class Program
{
    static void Main()
    {
        MusicalInstrument[] instruments = new MusicalInstrument[]
        {
            new Violin("Stradivarius", "Handcrafted masterpiece"),
            new Trombone("Bach", "Professional orchestral trombone"),
            new Ukulele("Kala", "Soprano ukulele"),
            new Cello("Montagnana", "High-quality cello")
        };
        foreach (var instrument in instruments)
        {
            instrument.Show();
            instrument.Sound();
            instrument.Desc();
            instrument.History();
            instrument.ShowInfo();
            Console.WriteLine();
        }
    }
}