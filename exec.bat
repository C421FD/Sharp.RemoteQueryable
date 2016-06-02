@Echo OFF
SET @main_py_path=%~dp0%\env\exec.py
SET @do_path=%~dp0%
SET @python_path=%~dp0%\env\Python35-32\python.exe 
chcp 850
%@python_path% %@main_py_path% %*
