# PhraseFinder

PhraseFinder es el resultado del desarrollo del Trabajo de Fin de Título _Detección automática de expresiones y locuciones en textos_, correspondiente a la titulación de Grado en Ingeniería Informática de la Universidad de Las Palmas de Gran Canaria. 

Se trata de una herramienta que detecta automáticamente expresiones y locuciones de la lengua española en cualquier texto. 
El sistema se divide en las siguientes aplicaciones:

## PhraseFinder.WPF

Aplicación de escritorio, solo compatible con Windows, que almacena la información de las locuciones y expresiones en una base de datos Microsoft Access. 
La aplicación puede ser ejecutada, a modo de prueba, habiendo instalado previamente Visual Studio 2022 y las herramientas de desarrollo de aplicaciones de escritorio que este proporciona. 
Puede ejecutarse desde la interfaz gráfica de Visual Studio, o mediante las siguientes órdenes en la terminal de Windows:

```
cd .\PhraseFinder.WPF\
dotnet run
```

Para poder probar todas las funciones disponibles en la aplicación, se ha proporcionado un fichero, denominado `DLE - shortened.txt`, que se encuentra en el directorio `Assets` del proyecto `PhraseFinder.WPF`.
Este archivo contiene una pequeña parte de la información del Diccionario de la lengua española y puede ser añadido y procesado en la aplicación de escritorio.

## PhraseFinder.WCF

Servicio web WCF que contiene la lógica de detección de las expresiones y locuciones. No puede ser ejecutado, pues depende de distintas librerías de código cerrado, propiedad del IATEXT, que no se encuentran en el repositorio. 

## PhraseFinder.WebApp

Aplicación web que permite al usuario introducir un texto de hasta 10.000 caracteres, o subir un fichero de texto de hasta 10KB. 
Tras ello, el sistema proporciona un informe con toda la información correspondiente a las expresiones y locuciones de la lengua española encontradas en el texto introducido.

A continuación, se muestra un ejemplo de detección en un texto que contiene una gran cantidad de expresiones y locuciones:

![resultado-final](https://github.com/Alejandro-M-Cruz/PhraseFinder/assets/113340373/412d1690-2707-4aa6-bbc0-01a7b44d94a8)

