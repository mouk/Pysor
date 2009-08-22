def add(name, service, impl, params={}):
	return addComponent(name, service, impl, params)
	
import clr
clr.AddReference("WindsorSample")
from WindsorSample import *


add( "StringParsingTitleScraper" , ITitleScraper, StringParsingTitleScraper)

	
add( "HttpFileDownloader", IFileDownloader, HttpFileDownloader)

add( "retriver", HtmlTitleRetriever, HtmlTitleRetriever)

add( "retriverWithParam", HtmlTitleRetriever, HtmlTitleRetriever, 
	{'AdditionalMessage': "Test"})

ftp = add( "ftp", FtpFileDownloader, FtpFileDownloader)

add( "ftpRetriver", HtmlTitleRetriever, HtmlTitleRetriever,
	{'downloader': ftp})

