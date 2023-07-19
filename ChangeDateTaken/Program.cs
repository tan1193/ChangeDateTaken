using ExifLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

Console.WriteLine("Enter your path: ");
string name = Console.ReadLine();
// step 1: get all images in folder
var files = System.IO.Directory.GetFiles(@$"{name}", "*.jpg");
// step 2: get date taken from  JSON file which has the same name as the image

foreach (var item in files)
{
    var jsonFile = System.IO.File.ReadAllText(@$"{item}.json");
    if (jsonFile is null)
    {
        continue;
    }
    // step 3: write date taken to image
    JObject jsonObject = JObject.Parse(jsonFile);
    long photoTakenTimestamp = (long)jsonObject["photoTakenTime"]["timestamp"];
    DateTime photoTakenDateTime = DateTimeOffset.FromUnixTimeSeconds(photoTakenTimestamp).UtcDateTime;
  
    var file = ImageFile.FromFile(item);
    file.Properties.Set(ExifTag.DateTime, photoTakenDateTime);
    file.Properties.Set(ExifTag.DateTimeDigitized, photoTakenDateTime);
    file.Properties.Set(ExifTag.DateTimeOriginal, photoTakenDateTime);
    // step 4: save image
    file.Save(item);
}
