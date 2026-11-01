<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="html" encoding="utf-8" indent="yes"/>

	<!-- Головний шаблон для кореневого елемента FacultyArchive (як на слайді 34) -->
	<xsl:template match="FacultyArchive">
		<HTML>
			<HEAD>
				<meta charset="utf-8"/>
				<TITLE>Електронний архів факультету</TITLE>
				<style>
					body { font-family: Segoe UI, Arial, sans-serif; margin: 25px; background-color: #fafafa; }
					h2 { color: #800000; }
					table { border-collapse: collapse; width: 100%; background-color: #ffffff; }
					th, td { padding: 8px 12px; text-align: left; }
					th { background-color: #f0e6e6; }
				</style>
			</HEAD>
			<BODY>
				<p>
					<H2>Електронний архів факультету (КНУ імені Тараса Шевченка)</H2>
				</p>
				<TABLE BORDER="2">
					<TR>
						<TH>
							<b>Автор</b>
						</TH>
						<TH>
							<b>Факультет</b>
						</TH>
						<TH>
							<b>Кафедра</b>
						</TH>
						<TH>
							<b>Вид матеріалу</b>
						</TH>
						<TH>
							<b>Мова</b>
						</TH>
						<TH>
							<b>Назва матеріалу</b>
						</TH>
						<TH>
							<b>Обсяг</b>
						</TH>
						<TH>
							<b>Дата створення</b>
						</TH>
					</TR>
					<xsl:apply-templates select="Material"/>
				</TABLE>
			</BODY>
		</HTML>
	</xsl:template>

	<!-- Шаблон для кожного елемента Material (як на слайдах 25 та 34) -->
	<xsl:template match="Material">
		<TR>
			<TD>
				<b>
					<xsl:value-of select="@Author"/>
				</b>
			</TD>
			<TD>
				<xsl:value-of select="@Faculty"/>
			</TD>
			<TD>
				<xsl:value-of select="@Department"/>
			</TD>
			<TD>
				<xsl:value-of select="@MaterialType"/>
			</TD>
			<TD>
				<xsl:if test="@Language">
					<xsl:value-of select="@Language"/>
				</xsl:if>
				<xsl:if test="not(@Language)">
					<xsl:text>—</xsl:text>
				</xsl:if>
			</TD>
			<TD>
				<xsl:value-of select="Details/Title"/>
			</TD>
			<TD>
				<xsl:value-of select="Details/Volume"/>
			</TD>
			<TD>
				<xsl:value-of select="Details/CreationDate"/>
			</TD>
		</TR>
	</xsl:template>
</xsl:stylesheet>