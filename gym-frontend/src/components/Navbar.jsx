import { Link, useNavigate } from "react-router-dom";

export default function Navbar() {
  const navigate = useNavigate();
  const token = localStorage.getItem("token");

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  return (
    <nav className="bg-white border-b shadow-sm">
      <div className="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center">
        {/* Logo */}
        <div className="flex items-center gap-2">
          <div className="w-8 h-8 bg-orange-500 rounded-md flex items-center justify-center text-white font-bold text-lg">
            💪
          </div>
          <Link
            to="/"
            className="text-xl font-semibold text-gray-800 hover:text-orange-500 transition"
          >
            SPORTIFY
          </Link>
        </div>

        {/* Right side menu */}
        <div className="space-x-4">
          {!token ? (
            <>
              <Link
                to="/login"
                className="text-gray-700 font-medium hover:text-orange-500 transition"
              >
                Sign In
              </Link>
              <Link
                to="/register"
                className="bg-orange-500 text-white px-4 py-2 rounded-lg font-semibold hover:bg-orange-600 transition"
              >
                Sign Up
              </Link>
            </>
          ) : (
            <>
              <Link
                to="/profile"
                className="text-gray-700 font-medium hover:text-orange-500 transition"
              >
                Profile
              </Link>
              <button
              onClick={handleLogout}
              className="bg-yellow-500 text-white px-4 py-2 rounded-lg font-semibold hover:bg-yellow-600 transition" 
              >
              Logout 
            </button>
            </>
          )}
        </div>
      </div>
    </nav>
  );
}
