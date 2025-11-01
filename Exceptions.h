#ifndef EXCEPTIONS_H
#define EXCEPTIONS_H
#include <stdexcept>
#include <string>
using namespace std;
class ProviderException : public runtime_error {
public:
    ProviderException(const string& message) : runtime_error(message) {}
};
class ClientException : public ProviderException {
public:
    ClientException(const string& message) : ProviderException(message) {}
};
#endif