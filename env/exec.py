import sys
import xml.etree.ElementTree as ET
import subprocess
import codecs
import settings
import colorama
from colorama import Fore, Back, Style
from os import path
import pdb

ms_build_paths = None
solution_path = None
paths = None
test_config = None
test_containers = None
mstest_path = 'env\mstest.exe'
build_command_template = "\"{0}\" {1} /t:{2} /p:Configuration={3} /consoleloggerparameters:Summary /verbosity:minimal"
clean_command_temlate = "\"{0}\" {1} /t:{2} /p:Configuration={3} /consoleloggerparameters:Summary /verbosity:minimal"
test_command_template = "\"{0}\" /testcontainer:{1} /testsettings:{2}"
colorama.init()


def get_command_arguments():
    copy_args = list(sys.argv)
    copy_args.remove(sys.argv[1])
    copy_args.remove(sys.argv[0])
    return copy_args


def initialize_settings(settings_file_path):
    tree = ET.parse(settings_file_path)
    root = tree.getroot()

    global ms_build_paths
    global solution_path
    global paths
    global test_config
    global test_containers

    ms_build_paths = settings.get_ms_build_paths(root)
    solution_path = root.find('SolutionName').text
    paths = settings.get_paths(root)
    test_config = root.find('Tests').get('ConfigPath')
    test_containers = settings.get_test_containers(root)


def main():
    try:
        initialize_settings('env\env_settings.xml')
        handlers = {'build': build, 'rebuild': rebuild, 'clean': clean, 'test': test }
        if len(sys.argv) <= 1:
            print('Command did not put')
            return

        if not sys.argv[1] in handlers:
            print('Handler for command {0} not found'.format(sys.argv[1]))
            return

        handler = handlers.get(sys.argv[1])
        args = get_command_arguments()
        handler(args)

    except Exception:
        print(Fore.RED + Back.BLACK + 'During executing command occurred exception:' + Style.RESET_ALL)
        print(sys.exc_info())


def rebuild(arguments):
    clean(arguments)
    build(arguments)


def clean(arguments):
    print(Fore.BLACK + Back.LIGHTGREEN_EX + 'Clean project' + Style.RESET_ALL)
    execute_ms_build_command(arguments, 'clean')


def build(arguments):
    print(Fore.BLACK + Back.LIGHTGREEN_EX+ 'Build project' + Style.RESET_ALL)
    execute_ms_build_command(arguments, 'build')


def execute_ms_build_command(arguments, action):
    ms_build_path = ms_build_paths.get('x86')
    config = 'debug'

    for arg in arguments:
        arg = str.lower(arg)
        if arg == 'x86' or arg == 'x64':
            ms_build_paths.get('x86')

        if arg == 'debug' or arg == 'release':
            config = arg

    command = clean_command_temlate.format(ms_build_path, solution_path, action, config)
    print(Fore.LIGHTGREEN_EX + Back.BLACK + 'Invoke: {0}'.format(command) + Style.RESET_ALL)
    result_code = subprocess.call(command, shell=True)
    if result_code != 0:
        raise Exception("MSBuild internal error, check the output")


def test(arguments):
    rebuild(arguments)
    config = 'debug'
    for arg in arguments:
        arg = str.lower(arg)
        if arg == 'debug' or arg == 'release':
            config = arg

    bin_path = path.join(paths.get(config), paths.get('innertest'))

    for test_container in test_containers:
        test_container_path = path.join(bin_path, test_container)
        test_config_path = path.join(bin_path, test_config)
        test_cmd = test_command_template.format(mstest_path, test_container_path, test_config_path)
        print(Fore.LIGHTGREEN_EX + Back.BLACK + 'Invoke: {0}'.format(test_cmd) + Style.RESET_ALL)
        result_code = subprocess.call(test_cmd, shell=True)
        if result_code != 0:
            raise Exception("MSTest internal error, check the output")


main()
