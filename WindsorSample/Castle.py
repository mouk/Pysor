
clr.AddReference("WindsorSample")
clr.AddReference("Tests")

from WindsorSample import *
from Tests import *






add( "StringParsingTitleScraper" , ITitleScraper, StringParsingTitleScraper)

	
add( "HttpFileDownloader", IFileDownloader, HttpFileDownloader)

add( "retriver", HtmlTitleRetriever, HtmlTitleRetriever)

add( "retriverWithParam", HtmlTitleRetriever, HtmlTitleRetriever, 
	{'AdditionalMessage': "Test"})

ftp = add( "ftp", FtpFileDownloader, FtpFileDownloader)

add( "ftpRetriver", HtmlTitleRetriever, HtmlTitleRetriever,
	{'downloader': ftp})

