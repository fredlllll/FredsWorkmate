import shutil

def replace_in_file(file_path:str, search:str, replace:str):
	with open(file_path, 'r') as file:
		filedata = file.read()

	filedata = filedata.replace(search,replace)

	with open(file_path, 'w') as file:
		file.write(filedata)

type_name = input("Give me the Type Name e.g. Address:")

shutil.copytree("BankInformation/",f"{type_name}/")

search = "BankInformation"
replace_in_file(f"./{type_name}/Details.cshtml",search,type_name)
replace_in_file(f"./{type_name}/Details.cshtml.cs",search,type_name)
replace_in_file(f"./{type_name}/Edit.cshtml",search,type_name)
replace_in_file(f"./{type_name}/Edit.cshtml.cs",search,type_name)
replace_in_file(f"./{type_name}/Index.cshtml",search,type_name)
replace_in_file(f"./{type_name}/Index.cshtml.cs",search,type_name)
