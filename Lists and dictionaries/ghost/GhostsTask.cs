using System;
using System.Text;

namespace hashes
{
    public class GhostsTask :
        IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
        IMagic
    {
        byte[] content;
        Document document;
        Vector vector = new Vector(9, 10);
        Segment segment = new Segment(new Vector(2, 2), new Vector(0, 4));
        Cat cat = new Cat("Cattttttt", "theBest", DateTime.Today);
        Robot robot = new Robot("111");

        Document IFactory<Document>.Create() => document;

        Vector IFactory<Vector>.Create() => vector;

        Segment IFactory<Segment>.Create() => segment;

        Cat IFactory<Cat>.Create() => cat;

        Robot IFactory<Robot>.Create() => robot;

        public GhostsTask()
        { //completeing doc creation
            var encoding = Encoding.ASCII;
            content = encoding.GetBytes("Happy People");
            document = new Document("docName", encoding, content);
        }

        public void DoMagic()
        {
            for (var i = 0; i < content.Length; i++)
                content[i] += 4;
            vector.Add(new Vector(8, 9));
            segment.Start.Add(new Vector(10, 11));
            Robot.BatteryCapacity *= 5;
            cat.Rename("Catty");
        }
    }
}