# -*- coding: utf-8 -*-

from datetime import datetime, date, timedelta, time


### -------------------------- DATE ENVIRONMENT
now = datetime.now()
start_year = None

month = now.month
if month>=9 : #setpember->december
    start_year = now.year
else :#january->august
    start_year = now.year - 1


class CustomSettings:
    START_DATE_CHEKING_PUNISHMENT = date(start_year, 9, 5) #years/month/day
    START_DATE_CHECKING_NON_CLOSED_ELEMENTS = date(start_year, 9, 5) #years/month/day
    DAYS_BEFORE_CHECKING_SCHOOL_LIFE_ABSENCE = 3
    DAYS_BEFORE_CHECKING_SCHOOL_PUNISHMENT = 1