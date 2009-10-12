
	
import clr
clr.AddReference("mscorlib")

import System
from System import Object



def add(name, service, impl, params={},lifestyle=""):
	return addComponent(name, service, impl, params,lifestyle)
	
def array(* params):
	return System.Array[Object](params)