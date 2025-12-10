import argparse
from pikepdf import Pdf, AttachedFileSpec, Name, Dictionary, Array
from pathlib import Path

parser = argparse.ArgumentParser()
parser.add_argument("pdf")
parser.add_argument("xml")
parser.add_argument("out")
args = parser.parse_args()

pdf = Pdf.open(args.pdf)
filespec = AttachedFileSpec.from_filepath(pdf, Path(args.xml))
filespec.filename = "xrechnung.xml"
pdf.attachments["xrechnung.xml"] = filespec
pdf.save(args.out)