
	
import clr
clr.AddReference("mscorlib")

import System
from System import Object



def add(name, service, impl, params={}):
	return addComponent(name, service, impl, params)
	
def array(* params):
	return System.Array[Object](params)