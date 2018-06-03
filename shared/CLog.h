#pragma once
#ifndef _IOLOG_
#define _IOLOG_

#define _O_LOG_DEBUG "[Debug] "
#define _O_LOG_ERROR "[Error] "
#define _O_LOG_INFO "[Info] "

#if !defined localtime_s 
#include <time.h>
#endif

class my_ostream
{
public:
	static std::string LogFile(const char* fileName = nullptr)
	{
		static std::string fname;
		if(fileName != nullptr)
			fname = fileName;
		return fname;
	}

	my_ostream() {
		my_fstream = std::ofstream(my_ostream::LogFile(), std::ios_base::app);
		my_fstream << std::endl;
	};

	my_ostream& info()
	{
		my_fstream << _O_LOG_INFO;
		return *this;
	}
	my_ostream& debug()
	{
		my_fstream << _O_LOG_DEBUG;
		return *this;
	}
	my_ostream& error()
	{
		my_fstream << _O_LOG_ERROR;
		return *this;
	}
	template<typename T> my_ostream& operator<<(const T& something)
	{
		my_fstream << something;
		return *this;
	}
	typedef std::ostream& (*stream_function)(std::ostream&);
	my_ostream& operator<<(stream_function func)
	{
		func(my_fstream);
		return *this;
	}
	static my_ostream& _log()
	{
		static my_ostream log_stream;
		time_t rawtime;
		tm timeinfo;
		char buffer[80];
		time(&rawtime);
		errno_t error = localtime_s(&timeinfo, &rawtime);
		if (error)
			return log_stream;
		strftime(buffer, 80, "[%Ex %EX]", &timeinfo);
		log_stream << buffer << " ";
		return log_stream;
	}
private:
	std::ofstream my_fstream;
};

#define log my_ostream::_log()
#define log_info my_ostream::_log().info()
#define log_debug my_ostream::_log().debug()
#define log_error my_ostream::_log().error()

#endif