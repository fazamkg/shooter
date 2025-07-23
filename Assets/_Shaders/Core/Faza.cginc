#ifndef FAZA_INCLUDED
#define FAZA_INCLUDED

static const fixed PI = 3.14159265h;
static const fixed PI_120 = 2.09439510h;
static const fixed PI_80 = 1.39626340h;

static const fixed C1 = 1.70158h;
static const fixed C2 = 2.70158h;
static const fixed C3 = 2.59490h;
static const fixed N1 = 7.5625h;
static const fixed D1 = 2.75h;

fixed easeInSine(fixed x)
{
    return 1.0h - cos(x * PI * 0.5h);
}

fixed easeInCubic(fixed x)
{
    return x * x * x;
}

fixed easeInQuint(fixed x)
{
    return x * x * x * x * x;
}

fixed easeInCirc(fixed x)
{
    return 1.0h - sqrt(1.0h - x * x);
}

fixed easeInElastic(fixed x)
{
    return x == 0.0h
        ? 0.0h
        : x == 1.0h
        ? 1.0h
        : -pow(2.0h, 10.0h * x - 10.0h) * sin((x * 10.0h - 10.75h) * PI_120);
}

fixed easeOutSine(fixed x)
{
    return sin(x * PI * 0.5);
}

fixed outCubic(fixed x)
{
    fixed a = 1.0h - x;
    return 1.0h - a * a * a;
}

fixed easeOutQuint(fixed x)
{
    fixed a = 1.0h - x;
    return 1.0h - a * a * a * a * a;
}

fixed easeOutCirc(fixed x)
{
    fixed a = x - 1.0h;
    return sqrt(1.0h - a * a);
}

fixed easeOutElastic(fixed x)
{
    return x == 0.0h
        ? 0.0h
        : x == 1.0h
        ? 1.0h
        : pow(2.0h, -10.0h * x) * sin((x * 10.0h - 0.75h) * PI_120) + 1.0h;
}

fixed easeInOutSine(fixed x)
{
    return -(cos(PI * x) - 1.0h) * 0.5h;
}

fixed easeInOutCubic(fixed x)
{
    return x < 0.5h ? 4.0h * x * x * x : 1.0h - pow(-2.0h * x + 3.0h, 3) * 0.5h;
}

fixed easeInOutQuint(fixed x)
{
    return x < 0.5h ? 16.0h * x * x * x * x * x : 1.0h - pow(-2.0h * x + 2.0h, 5) * 0.5h;
}

fixed easeInOutCirc(fixed x)
{
    return x < 0.5h ? (1.0h - sqrt(1.0h - (2.0h * x) * (2.0h * x))) * 0.5h
        : (sqrt(1.0h - (-2.0h * x + 2.0h) * (-2.0h * x + 2.0h)) + 1.0h) * 0.5h;
}

fixed easeInOutElastic(fixed x)
{
    return x == 0.0h
        ? 0.0h
        : x == 1.0h
        ? 1.0h
        : x < 0.5h
        ? -(pow(2.0h, 20.0h * x - 10.0h) * sin((20.0h * x - 11.125h) * PI_80)) * 0.5h
        : (pow(2.0h, -20.0h * x + 10.0h) * sin((20.0h * x - 11.125h) * PI_80)) * 0.5h + 1.0h;
}

fixed easeInQuad(fixed x)
{
    return x * x;
}

fixed easeInQuart(fixed x)
{
    return x * x * x * x;
}

fixed easeInExpo(fixed x)
{
    return x == 0.0h ? 0.0h : pow(2.0h, 10.0h * x - 10.0h);
}

fixed easeInBack(fixed x)
{
    return C2 * x * x * x - C1 * x * x;
}

fixed easeOutBounce(fixed x)
{
    if (x < 1.0h / D1)
    {
        return N1 * x * x;
    }

    if (x < 2.0h / D1)
    {
        return N1 * (x -= 1.5h / D1) * x + 0.75h;
    }

    if (x < 2.5h / D1)
    {
        return N1 * (x -= 2.25h / D1) * x + 0.9375h;
    }

    return N1 * (x -= 2.625h / D1) * x + 0.984375h;
}

fixed easeInBounce(fixed x)
{
    return 1.0h - easeOutBounce(1.0h - x);
}

fixed easeOutQuad(fixed x)
{
    return 1.0h - (1.0h - x) * (1.0h - x);
}

fixed easeOutQuart(fixed x)
{
    fixed a = 1.0h - x;
    return 1.0h - a * a * a * a;
}

fixed easeOutExpo(fixed x)
{
    return x == 1.0h ? 1.0h : 1.0h - pow(2.0h, -10.0h * x);
}

fixed easeOutBack(fixed x)
{
    fixed a = x - 1.0h;
    return 1.0h + C2 * a * a * a + C1 * a * a;
}

fixed easeInOutQuad(fixed x)
{
    fixed a = -2.0h * x + 2.0h;
    return x < 0.5h ? 2.0h * x * x : 1.0h - a * a * 0.5h;
}

fixed easeInOutQuart(fixed x)
{
    fixed a = -2.0h * x + 2.0h;
    return x < 0.5h ? 8.0h * x * x * x * x : 1.0h - a * a * a * a * 0.5h;
}

fixed easeInOutExpo(fixed x)
{
    return x == 0.0h
        ? 0.0h
        : x == 1.0h
        ? 1.0h
        : x < 0.5h ? pow(2.0h, 20.0h * x - 10.0h) * 0.5h
        : (2.0h - pow(2.0h, -20.0h * x + 10.0h)) * 0.5h;
}

fixed easeInOutBack(fixed x)
{
    return x < 0.5h
        ? (pow(2.0h * x, 2.0h) * ((C3 + 1.0h) * 2.0h * x - C3)) * 0.5h
        : (pow(2.0h * x - 2.0h, 2.0h) * ((C3 + 1.0h) * (x * 2.0h - 2.0h) + C3) + 2.0h) * 0.5h;
}

fixed easeInOutBounce(fixed x)
{
    return x < 0.5h
        ? (1.0h - easeOutBounce(1.0h - 2.0h * x)) * 0.5h
        : (1.0h - easeOutBounce(2.0h * x - 1.0h)) * 0.5h;
}

#endif