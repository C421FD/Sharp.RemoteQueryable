from enum import Enum
import xml.etree.ElementTree as ET


def get_ms_build_paths(root):
    result = {}
    ms_build_section = root.find('MSBuild')
    for ms_build_path in ms_build_section:
        result[ms_build_path.get('target')] = ms_build_path.text

    return result


def get_paths(root):
    result = {}
    paths_section = root.find('Paths')
    for ms_build_path in paths_section:
        result[str(ms_build_path.tag).lower()] = ms_build_path.text

    return result


def get_test_containers(root):
    tests_section = root.find('Tests')
    for test_container_path in tests_section:
        yield test_container_path.text
