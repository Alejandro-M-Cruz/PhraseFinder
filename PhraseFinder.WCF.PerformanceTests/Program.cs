using System.Diagnostics;
using PhraseFinder.WCF.PerformanceTests;
using PhraseFinderServiceReference;
using ServicioLematizacion;

//var client = new PhraseFinderServiceClient();

//var text = 
//	"alguno, na que otro, tra texto de ejemplo.\n esto es un texto de ejemplo";

//var stopwatch = Stopwatch.StartNew();
//var foundPhrases = await client.FindPhrasesAsync(text);
//stopwatch.Stop();

//Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");

//foreach (var phrase in foundPhrases)
//{
//	Console.WriteLine($"Phrase: {phrase.Phrase}");
//	Console.WriteLine($"Start index: {phrase.StartIndex}");
//	Console.WriteLine($"End index: {phrase.EndIndex}");
//	Console.WriteLine($"Length: {phrase.Length}");

//	foreach (var definitionToExamples in phrase.DefinitionToExamples)
//	{
//		Console.WriteLine($"Definition: {definitionToExamples.Key}");
//		foreach (var example in definitionToExamples.Value)
//		{
//			Console.WriteLine($"Example: {example}");
//		}
//	}
//}

// text of example

var sampleText1 = 
    "Por cierto, sentémonos a la mesa y se dieron cuenta enseguida. Había comida a montones. " + 
    "Al poco tiempo, comenzaron  a almorzar y a echar un párrafo. " +
    "¿Cabría la posibilidad de ...?\n" +
    "El primero en irse fue Juan, que siempre actúa por cuenta propia.";

var sampleText2 = 
    "Tanto es así que, sin pensarlo dos veces,¡cáspitas!¡Desde luego!";

var sampleText3 =
    "Eran las tres de la tarde y hacía un calor de mil demonios. Juan, que no daba pie con bola en su trabajo, decidió que era hora de tomarse un respiro. \"Voy a salir a despejarme un rato\", pensó, y sin pensarlo dos veces, se dirigió al parque que estaba a tiro de piedra de su oficina.\r\n\r\nAl llegar, se encontró con su amigo Pedro, que estaba de capa caída porque acababa de perder su empleo. \"¡Vaya palo!\", exclamó Juan. \"Pero ánimo, que no hay mal que por bien no venga. Seguro que encuentras algo mejor\".\r\n\r\nSe sentaron en un banco a charlar y ponerse al día. Pedro le contó que andaba con la soga al cuello porque las cuentas no le cuadraban y tenía que apretarse el cinturón. Juan, por su parte, no estaba para tirar cohetes; el trabajo le traía por la calle de la amargura y encima tenía una discusión pendiente con su jefe, que no le daba ni la hora.\r\n\r\n\"Bueno, ya sabes lo que dicen, a mal tiempo, buena cara\", dijo Pedro, intentando levantar el ánimo. \"Además, cuando una puerta se cierra, otra se abre\".\r\n\r\nDecidieron dar una vuelta y, para matar el tiempo, entraron en una cafetería. Al pedir, la camarera les dio largas porque había mucha gente, pero ellos se lo tomaron con filosofía. \"A caballo regalado no le mires el diente\", bromeó Juan cuando la camarera les trajo dos cafés como si fueran un tesoro.\r\n\r\nAl rato, Pedro recibió una llamada. \"¡Anda, mira quién fue a hablar! El jefe de la empresa en la que había hecho una entrevista hace una semana\". Después de colgar, Pedro tenía una sonrisa de oreja a oreja. \"Me han contratado\", dijo, sin poder creer su buena suerte.\r\n\r\n\"¡Qué buena onda!\", exclamó Juan. \"Ya ves, después de la tormenta siempre viene la calma\".\r\n\r\nLos dos amigos brindaron por los nuevos comienzos y, aunque sabían que la vida no es color de rosa, estaban decididos a enfrentar lo que viniera con optimismo. Porque al fin y al cabo, más vale tarde que nunca y, en las buenas y en las malas, siempre podrían contar el uno con el otro.";

await Utils.PrintInfo("ha echado una mano a ese tipo");
