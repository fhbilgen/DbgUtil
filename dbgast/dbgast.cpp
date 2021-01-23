// dbgast.cpp : This file contains the 'main' function. Program execution begins and ends there.
//


#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>
#include <string.h>

#define MXTMLN 30   //  maximum time string lenght e.g. "000 days 23:52:15.999"

char* s_gets(char* st, int n);
void eatline();
unsigned long procTime2Sec(const char* tmstr);

int main()
{
    char procTime[MXTMLN];
    printf("Process Uptime: ");
    s_gets(procTime, MXTMLN);
}

char* s_gets(char* st, int n)
{
    char* retval;
    char* find;

    retval = fgets(st, n, stdin);

    if (retval)
    {
        find = strchr(st, '\n');
        if (find)
            *find = '\0';
        else
            while (getchar() != '\n')
                continue;
    }

}



void eatline()
{
    while (getchar() != '\n')
        continue;
}


unsigned long procTime2Sec(const char* tmstr)
{
    unsigned long retval = 0;

    return retval;
}