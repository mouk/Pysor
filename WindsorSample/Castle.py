
clr.AddReference("WindsorSample")
clr.AddReference("Tests")

from WindsorSample import *
from Tests import *






add( "MessageStorage" , MessageStorage, MessageStorage, {'messages':['first message', 'second message' ]})


add( "StringParsingTitleScraper" , ITitleScraper, StringParsingTitleScraper)

	
downloader = add( "HttpFileDownloader", IFileDownloader, HttpFileDownloader)

add( "retriver", HtmlTitleRetriever, HtmlTitleRetriever)

add( "retriverWithParam", HtmlTitleRetriever, HtmlTitleRetriever, 
	{'AdditionalMessage': "Test"})

ftp = add( "ftp", FtpFileDownloader, FtpFileDownloader)



add( "ftpRetriver", HtmlTitleRetriever, HtmlTitleRetriever,
	{'downloader': ftp})
	
dings = add( "DowloaderStorage", DowloaderStorage, DowloaderStorage, {'dowloader' : ftp})



add( "MultipleDowloaderStorage", MultipleDowloaderStorage, MultipleDowloaderStorage, {'dowloaders' : [ ftp] })

add("transient" , Object, Object,lifestyle="transient")

add( "SimpleClass", SimpleClass, SimpleClass, {'IntegerValue' : 10, 'BooleanValue': True})