import { Link, useLocation } from "react-router-dom";

export default function Navbar() {
  const { pathname } = useLocation();
  const isHome = pathname === "/";

  return (
    <nav
      className={`fixed top-0 left-0 w-full z-50 flex items-center justify-between px-10 py-5 font-[Poppins] transition-all duration-300 ${
        isHome ? "bg-transparent" : "bg-black/90 backdrop-blur-lg border-b border-neutral-800"
      }`}
    >
      <Link
        to="/"
        className="text-white text-3xl font-extrabold tracking-wide hover:text-yellow-400 transition"
      >
        SPORTIFY
      </Link>
      <div className="flex items-center gap-6">
        <Link
          to="/login"
          className="text-white text-lg font-medium hover:text-yellow-400 transition"
        >
          Sign In
        </Link>
        <Link
          to="/register"
          className="bg-yellow-500 text-black px-5 py-2 rounded-full text-lg font-semibold hover:bg-yellow-400 transition"
        >
          Sign Up
        </Link>
      </div>
    </nav>
  );
}
